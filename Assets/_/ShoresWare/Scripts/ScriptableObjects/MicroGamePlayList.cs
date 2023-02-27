using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShoresWare.ScriptableObjects {
    [CreateAssetMenu(fileName = "MicroGamePlayList", menuName = "ShoresWare/MicroGame PlayList", order = 1)]
    public class MicroGamePlayList : ScriptableObject {
        public Data.ItemInfo itemInfo = new Data.ItemInfo {
            name = "Undefined",
            description = "An undefined MicroGame!",
            authorName = "Author Unknown"
        };

        public Texture2D previewImage;

        public List<MicroGameInfo> microGames = new List<MicroGameInfo>();

        public Data.FrameInfo frameInfo = new Data.FrameInfo {
            borderColor = Color.white,
            backgroundColor = Color.gray
        };
    }
}
