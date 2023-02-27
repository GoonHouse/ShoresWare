using UnityEngine;
using UnityEngine.Events;

namespace ShoresWare.Core {
    public class MicroGameObject : MonoBehaviour {
        [System.Serializable]
        public class BeatEvent : UnityEvent<int> {}
        
        public UnityEvent<int> onMicroGameStart;
        public UnityEvent onMicroGameEnd;
        public BeatEvent onBeat;
        public BeatEvent onLateBeat;
        public BeatEvent onBar;
        public BeatEvent onLateBar;
        public UnityEvent onWin;
        public UnityEvent onLose;


        // called before a microgame begins
        public virtual void OnMicroGameStart(int lengthInBeats, Data.GameDifficulty difficulty) {
            onMicroGameStart?.Invoke(lengthInBeats);
        }
        
        // called right before a microgame goes from done to exiting
        public virtual void OnMicroGameEnd() {
            onMicroGameEnd?.Invoke();
        }
        
        
        // called globally on each beat
        // unknown script execution order
        public virtual void OnBeat(int beatNumber) {
            onBeat?.Invoke(beatNumber);
        }
        
        // called globally after OnBeat finishes
        // use for logic that may depend on another MicroGameObject using OnBeat
        public virtual void OnLateBeat(int beatNumber) {
            onLateBeat?.Invoke(beatNumber);
        }
        
        public virtual void OnBar(int barNumber) {
            onBar?.Invoke(barNumber);
        }
        
        public virtual void OnLateBar(int barNumber) {
            onLateBar?.Invoke(barNumber);
        }

        public virtual void OnWin() {
            onWin?.Invoke();
        }

        public virtual void OnLose() {
            onLose?.Invoke();
        }

        public virtual void Win() {
            var elev = FindObjectOfType<Elevator>();
            elev.Win();
        }

        public virtual void Lose() {
            var elev = FindObjectOfType<Elevator>();
            elev.Lose();
        }
    }
}