using ShoresWare.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ShoresWare.Games._DEBUG.DBG_IMAGESYNC {
    public class BeatColors : MicroGameObject {
        public Image _image;
    
        public override void OnMicroGameStart(int lengthInBeats, Data.GameDifficulty difficulty) {
            base.OnMicroGameStart(lengthInBeats, difficulty);
            _image = GetComponent<Image>();
        }

        public override void OnBeat(int beatNumber) {
            base.OnBeat(beatNumber);
            if (_image != null) {
                _image.color = new Color(Random.value, Random.value, Random.value);                
            }
        }
    }
}
