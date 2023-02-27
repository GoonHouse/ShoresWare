using System;
using System.Collections.Generic;
using System.Linq;
using BeeCore.Extensions;
using ShoresWare.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace ShoresWare.Core {
    public class BoomBox : MonoBehaviour {
        public const int SECONDS_PER_MINUTE = 60;
        public const int BEATS_PER_BAR = 4;
        public const int CROTCHETS_PER_BAR = 8;
        
        // @TODO: enforce that you're able to work within these BMP ranges within a microgame
        // e.g.: you don't hit the true max during gameplay, but a microgame can always get faster to these limits
        public const int MIN_BPM = 60;
        public const int DEFAULT_BPM = 120;
        public const int MAX_BPM = 240;
        
        public AudioSource musicSource;
        public float bpm = 140.0f;
        public float songStartOffset = 0.2f; // positive means the song must be minussed!
        private double songStartDSP;
        public float songPosition = 0.0f;
        public MusicTrack musicTrack;

        public List<MusicTrack> randomTracks = new List<MusicTrack>();
        
        [SerializeField] private Metronome _metronome;
        
        
        public Dictionary<int, MicroGameObject.BeatEvent> scheduledBeats = new Dictionary<int, MicroGameObject.BeatEvent>();
        public MicroGameObject.BeatEvent scheduledBar = new MicroGameObject.BeatEvent(); 
        
        public int beatNumber = 0;
        public int barNumber = 0;
        public float lastBeat;
        [HideInInspector] public float crotchet;

        public Image beatImage;

        public TextMeshProUGUI debugText;
        
        // Start is called before the first frame update
        void Start(){
            SetMusicTrack(musicTrack);
        }

        // for use when BPM changes or for downscaling songs
        void SetPitch(float pitch) {
            var oldPitch = musicSource.pitch;
            musicSource.pitch = pitch;
            float pitchRatio = pitch / oldPitch;
            lastBeat *= pitchRatio;
//            _metronome.nextTick *= (double)pitchRatio / _metronome.sampleRate;
            UpdateDebugText();
        }
        
        // for arbitrarily assigning the BPM mid-song
        public void SetBPM(float newBPM) {
            newBPM = Mathf.Max(Mathf.Min(newBPM, MAX_BPM), MIN_BPM);
            SetPitch(newBPM / musicTrack.bpm);
            bpm = Mathf.RoundToInt(newBPM);
            crotchet =  SECONDS_PER_MINUTE / bpm;
            _metronome = FindObjectOfType<Metronome>();
            if (_metronome != null) {
                _metronome.bpm = bpm;                
            }

            UpdateDebugText();
            CalcSongPosition();
        }

        public void ScheduleOnRelativeBeat(int inBeats, UnityAction<int> unityEvent) {
            Debug.Log($"scheduling on {beatNumber} for something +{inBeats}");
            if (!scheduledBeats.ContainsKey(inBeats)) {
                scheduledBeats[inBeats] = new MicroGameObject.BeatEvent();
            }
            scheduledBeats[inBeats].AddListener(unityEvent);
        }
        
        
        #region Medical Advice
        // https://www.reddit.com/r/gamedev/comments/2fxvk4/heres_a_quick_and_dirty_guide_i_just_wrote_how_to/
        
        [HideInInspector] public float deltaSongPos;
        [HideInInspector] public float lastHit; // = 0.0f; // the last (snapped to the beat) time an input was pressed
        [HideInInspector] public float actualLastHit;
        [HideInInspector] private float nextBeatTime = 0.0f;
        [HideInInspector] private float nextBarTime = 0.0f;
        
        [HideInInspector] public float addOffset; // additional, per level offset.
        [HideInInspector] public static float offsetStatic = 0.40f;
        [HideInInspector] public static bool hasOffsetAdjusted = false;
        
        #endregion

        void Update(){
            CalcSongPosition();
            
            if ( songPosition > lastBeat + crotchet ) {
                var objects = FindObjectsOfType<MicroGameObject>();
                
                lastBeat += crotchet;
                beatNumber++;
                foreach (var o in objects) {
                    o.OnBeat(beatNumber);
                }
                foreach (var o in objects) {
                    o.OnLateBeat(beatNumber);
                }
                foreach (KeyValuePair<int,MicroGameObject.BeatEvent> keyValuePair in scheduledBeats.ToList()) {
//                    Debug.Log($"moving {keyValuePair.Key} to {(keyValuePair.Key - 1)}");
                    if( keyValuePair.Key - 1 <= 0 ){
                        keyValuePair.Value?.Invoke(beatNumber);
                        keyValuePair.Value?.RemoveAllListeners();
                        scheduledBeats.Remove(keyValuePair.Key);
                    } else {
                        scheduledBeats[keyValuePair.Key - 1] = keyValuePair.Value;
                        scheduledBeats.Remove(keyValuePair.Key);
                    }
                }
                
                if (beatNumber % BEATS_PER_BAR == 0) {
                    barNumber++;
                    scheduledBar.Invoke(beatNumber);
                    scheduledBar.RemoveAllListeners();
                    
                    foreach (var o in objects) {
                        o.OnBar(barNumber);
                    }
                    foreach (var o in objects) {
                        o.OnLateBar(barNumber);
                    }
                }
                
                UpdateDebugText();
            }

//            if (UnityEngine.Input.GetKeyDown(KeyCode.Q)) {
//                SetBPM(Mathf.RoundToInt(UnityEngine.Random.Range(60.0f, 240.0f)));
//            }
//            
//            if (UnityEngine.Input.GetKeyDown(KeyCode.T)) {
//                SetMusicTrack(randomTracks.Random());
//            }
//            
//            if (UnityEngine.Input.GetKeyDown(KeyCode.E)) {
//                var rel = Mathf.RoundToInt(UnityEngine.Random.Range(3, 9));
//                Debug.Log($"scheduled thing on {beatNumber} for +{rel}...");
//                ScheduleOnRelativeBeat(rel, arg0 => {
//                    var i = beatNumber;
//                    Debug.Log($"beat scheduled from {i} for +{rel} happening on {arg0}");
//                });
//            }
        }

        

        void UpdateDebugText() {
            debugText.text = $"BPM: {bpm} \nPitch: {musicSource.pitch} \nBeat #{beatNumber}";
        }

        public void UnscheduleAll() {
            scheduledBeats.Clear();
            scheduledBar.RemoveAllListeners();
        }

        public void SetMusicTrack(MusicTrack newMusicTrack) {
            musicTrack = newMusicTrack;
            songPosition = 0.0f;
            songStartOffset = musicTrack.offset;
            songStartDSP = AudioSettings.dspTime;
            beatNumber = 0;
            barNumber = 0;
            lastBeat = 0.0f;
            
            musicSource.Stop();
            SetBPM(bpm);
            musicSource.clip = musicTrack.clip;
            SetPitch(bpm / musicTrack.bpm);
            musicSource.Play();
        }

        void CalcSongPosition() {
            songPosition = (float) (AudioSettings.dspTime - songStartDSP) * musicSource.pitch - songStartOffset;
        }

        public void PlaySound(AudioClip sound) {
            var soundObj = new GameObject();
            soundObj.transform.SetParent(transform, true);
            soundObj.name = sound.name;
            var audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.pitch = bpm / DEFAULT_BPM;
            audioSource.Play();
            
            Destroy(soundObj, audioSource.clip.length * audioSource.pitch + Mathf.Epsilon);
        }
    }
}
