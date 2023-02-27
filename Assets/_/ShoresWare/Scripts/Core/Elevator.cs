using System;
using System.Collections;
using ShoresWare.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using BeeCore.Extensions;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// consider the following: https://github.com/sindrijo/unity3d-runtime-buildscenes
// on game over: Resources.UnloadUnusedAssets

namespace ShoresWare.Core {
    public class Elevator : MonoBehaviour {
        #region Scene hookups
        public PlayableDirector overlay; 
        public Dictionary<string, PlayableAsset> animations = new Dictionary<string, PlayableAsset>();
        public TextMeshProUGUI commandText;
        public GameObject winObject;
        public GameObject loseObject;
        public Image frameBorder;
        public Image frameBackground;
        #endregion

        public MusicTrack elevatorMusic;
        public MicroGamePlayList playList; 
        
        public Data.GameDifficulty difficulty = Data.GameDifficulty.GAME_DIFFICULTY_NONE;

        private string _command;

        public Data.GameState gameState = Data.GameState.GAME_STATE_INDETERMINANT;

        public AsyncOperation loadingScene;
        public int loadingBuildIndex;
        public int elevatorBuildIndex;
        public AsyncOperation unloadingScene;
        public AsyncOperation unloadingBuildIndex;
        private MicroGameInfo _microGameInfo;
        private Scene _scene;

        private void Start() {
            FindObjectOfType<BoomBox>().SetMusicTrack(elevatorMusic);
            StartCoroutine(EnterMicroGame(playList.microGames.Random()));
        }

        public void SetCommand(string command) {
            _command = command;
            commandText.text = _command;
            if (!string.IsNullOrWhiteSpace(_command)) {
                overlay.Stop();
                overlay.initialTime = 0;
                

                overlay.Play();                
            }
            // @TODO: play the animation for the command appearing by itself
        }

        public Scene GetSceneForDifficulty(Data.GameDifficulty _difficulty = Data.GameDifficulty.GAME_DIFFICULTY_NONE) {
            if (_difficulty == Data.GameDifficulty.GAME_DIFFICULTY_NONE) {
                _difficulty = difficulty;
            }

            return SceneManager.GetSceneByPath(_microGameInfo.difficultyMap[_difficulty].ScenePath);
        }

        /*
         * x 1) load the microgame scene on the correct difficulty
         * x 2) after scene async load, call OnMicroGameStart on each object
         * x 3) if no command text has been given on game start, then provide the default command text
         * x 4) play enter microgame animation
         * x 4.1) play music flare for game start (1 bar)
         * x 4.2) draw the instructions string, after reaches normal size, proceed:
         * 4.3) zoom in the elevator overlay and make invisible (overlaps with first beat of microgame song)
         * 4.4) reveal microgame overlay + hold command text briefly
         * 5) let microgame play out
         * 6) microgame is lost or won prematurely: fuse shortens to nearest bar
         * 7) 
         */
        public IEnumerator EnterMicroGame(MicroGameInfo gameInfo) {
            _microGameInfo = gameInfo;
            elevatorBuildIndex = SceneManager.GetActiveScene().buildIndex;
            gameState = Data.GameState.GAME_STATE_INDETERMINANT;
            winObject.SetActive(false);
            loseObject.SetActive(false);
            SetCommand(null);

//            _scene = GetSceneForDifficulty(difficulty);
            loadingBuildIndex = SceneUtility.GetBuildIndexByScenePath(_microGameInfo._sceneField.ScenePath);
            loadingScene = SceneManager.LoadSceneAsync(loadingBuildIndex, new LoadSceneParameters(LoadSceneMode.Additive, _microGameInfo.physicsMode));
            loadingScene.allowSceneActivation = false;
            yield return null;

            while (!loadingScene.isDone) {
                if (loadingScene.progress >= 0.9f) {
                    var boomBox = FindObjectOfType<BoomBox>();
                    boomBox.scheduledBar.AddListener(beat => {
                        loadingScene.allowSceneActivation = true;
                        loadingScene.completed += InnerLoad;
                        boomBox.UnscheduleAll();
                    });
                    yield break;
                }
                yield return null;
            }
        }

        public void SetFrameInfo(Data.FrameInfo frameInfo) {
            frameBackground.sprite = frameInfo.backgroundSprite ? frameInfo.backgroundSprite : frameBackground.sprite; 
            frameBackground.color = frameInfo.backgroundColor;
            frameBorder.sprite = frameInfo.borderSprite ? frameInfo.borderSprite : frameBorder.sprite;
            frameBorder.color = frameInfo.borderColor;
        }

        public void InnerLoad(AsyncOperation obj) {
            loadingScene.completed -= InnerLoad;
            _scene = SceneManager.GetSceneByBuildIndex(loadingBuildIndex);
                        
            if (SceneManager.SetActiveScene(_scene)) {
                // cosmetic shit
                SetFrameInfo(_microGameInfo.frameInfo);
                
                var allObjects = FindObjectsOfType<MicroGameObject>();
                foreach (var allObject in allObjects) {
                    // @TODO: by this point we're loaded and this is our first point to address each object in the scene 
//                    allObject.enabled = false;
                    allObject.OnMicroGameStart(_microGameInfo.lengthInBeats, difficulty);
                }
                

                // @TODO: play intro animation, defer everything after this point by one beat, activate objects again on first beat 
                // by now all the microgames should have processed at least once, so check the command
                SetCommand(_command ?? _microGameInfo.command);
                
                    
                var boomBox = FindObjectOfType<BoomBox>();
                // @TODO: make this play the command, intro start, then the level start sting
                boomBox.SetMusicTrack(_microGameInfo.musicTrack);

                if (_microGameInfo.lengthInBeats > 1) {
                    boomBox.ScheduleOnRelativeBeat(_microGameInfo.lengthInBeats + 1, endBeat => {
//                        Debug.Log("InnerLoad: microgame ended!");
                        StartCoroutine(ExitMicroGame());
//                        boomBox.UnscheduleAll();
                    });
                }
                
            } else {
                throw new System.NotImplementedException("could not activate scene, not sure what to do?");
            }
        }

        public IEnumerator ExitMicroGame() {
//            Debug.Log("ExitMicroGame: microgame over!");
            var allObjects = FindObjectsOfType<MicroGameObject>();
            foreach (var allObject in allObjects) {
                allObject.OnMicroGameEnd();
            }
            
            yield return new WaitForEndOfFrame();

            if (gameState != Data.GameState.GAME_STATE_WON) {
                Lose();
            }
            winObject.SetActive(false);
            loseObject.SetActive(false);
            SetCommand(null);
            
            var boomBox = FindObjectOfType<BoomBox>();
            boomBox.SetMusicTrack(elevatorMusic);

            _scene = SceneManager.GetSceneByBuildIndex(elevatorBuildIndex);
            SceneManager.SetActiveScene(_scene);
            unloadingScene = SceneManager.UnloadSceneAsync(loadingBuildIndex);
//            _scene = GetSceneForDifficulty(difficulty);
            unloadingScene.completed += InnerUnload;
        }
        
        void InnerUnload(AsyncOperation obj) {
//            Debug.Log("inner unload happened");
            unloadingScene.completed -= InnerUnload;
            
            SetFrameInfo(playList.frameInfo);
            // @TODO: ONLY IN HELL
//            var boomBox = FindObjectOfType<BoomBox>();
//            boomBox.SetBPM(boomBox.bpm+10);
            
            // @TODO: potentially an intro/outro here
            
            StartCoroutine(EnterMicroGame(playList.microGames.Random()));
        }

        public void Win() {
            if (gameState == Data.GameState.GAME_STATE_INDETERMINANT) {
                var allObjects = FindObjectsOfType<MicroGameObject>();
                foreach (var allObject in allObjects) {
                    allObject.OnWin();
                }

                winObject.SetActive(true);
            }
        }

        public void Lose() {
            if (gameState == Data.GameState.GAME_STATE_INDETERMINANT) {
                var allObjects = FindObjectsOfType<MicroGameObject>();
                foreach (var allObject in allObjects) {
                    allObject.OnLose();
                }
                loseObject.SetActive(true);
            }
        }
    }
}