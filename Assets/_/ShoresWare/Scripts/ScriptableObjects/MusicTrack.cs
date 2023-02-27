using UnityEngine;

namespace ShoresWare.ScriptableObjects {
    [CreateAssetMenu(fileName = "MusicTrack", menuName = "ShoresWare/MusicTrack", order = 1)]
    public class MusicTrack : ScriptableObject {
        public Data.ItemInfo itemInfo;

        [Tooltip("The music sample to be played.")]
        public AudioClip clip;
        [Tooltip("The BPM of the music, so that it can be scaled according to the game tempo.")]
        public float bpm;
        [Tooltip("The offset for the song start, in seconds.")]
        public float offset;
    }
}
