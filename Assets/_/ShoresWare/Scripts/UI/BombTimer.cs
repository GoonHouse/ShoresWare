using BeeCore;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace ShoresWare.UI {
    public class BombTimer : MicroGameObject {
        public RectTransform fuse;
        public PlayableDirector fire;
        public GameObject dynamic;
        private int stepBeat;
        public int bombFuseBeats = 8;

        public override void OnMicroGameStart(int lengthInBeats, Data.GameDifficulty difficulty) {
            base.OnMicroGameStart(lengthInBeats, difficulty);
            var boomBox = FindObjectOfType<BoomBox>();
            bombFuseBeats = lengthInBeats + 1;
            stepBeat = 0;
        }

        public override void OnBeat(int beatNumber) {
            base.OnBeat(beatNumber);
            stepBeat++;
        }

        private void Update() {
            if (stepBeat >= bombFuseBeats) {
                var elevator = FindObjectOfType<Elevator>();
            }

            if (stepBeat >= 2) {
                fuse.gameObject.SetActive(true);
                dynamic.SetActive(true);
                var anchorMax = fuse.anchorMax;
                anchorMax.x = MathBee.Scale(stepBeat, 2, (bombFuseBeats - 1), 1.0f, 0.13f);
                fuse.anchorMax = anchorMax;   
            } else {
                fuse.gameObject.SetActive(false);
                dynamic.SetActive(false);
            }
            
            
        }
    }
}