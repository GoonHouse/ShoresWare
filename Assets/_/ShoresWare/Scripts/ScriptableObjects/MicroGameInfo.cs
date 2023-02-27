using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShoresWare.ScriptableObjects {
    [CreateAssetMenu(fileName = "MicroGameInfo", menuName = "ShoresWare/MicroGame Info", order = 1)]
    public class MicroGameInfo : ScriptableObject {
        public Data.ItemInfo itemInfo = new Data.ItemInfo {
            name = "Undefined",
            description = "An undefined MicroGame!",
            authorName = "Author Unknown"
        };

        public Texture2D previewImage;
        public string command = "Do Something";

        public Data.ControlType controlType = Data.ControlType.CONTROL_TYPE_NONE;
        public MusicTrack musicTrack;

        
        public int lengthInBeats = (int)Data.GameLength.GAME_LENGTH_NORMAL;

        public Data.SceneField _sceneField;
        
        public Dictionary<Data.GameDifficulty, Data.SceneField> difficultyMap = new Dictionary<Data.GameDifficulty, Data.SceneField>();
        public LocalPhysicsMode physicsMode;

        public Data.FrameInfo frameInfo = new Data.FrameInfo {
            borderColor = Color.white,
            backgroundColor = Color.gray
        };
    }
}
