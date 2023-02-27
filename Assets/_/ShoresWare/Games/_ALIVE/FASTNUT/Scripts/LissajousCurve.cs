using UnityEngine;

// based on: https://codepen.io/leekee/pen/VKyKNz
// reference: https://en.wikipedia.org/wiki/Lissajous_curve

namespace ShoresWare.Games.FASTNUT {
    public class LissajousCurve : MonoBehaviour {
        public Vector2 multiplier = new Vector2(6, 7);
        public Vector2 size = new Vector2(300.0f, 200.0f);
        private Vector3 startPos;
        
        // Start is called before the first frame update
        void Start() {
            startPos = transform.localPosition;
        }

        // Update is called once per frame
        void Update() {
            transform.localPosition = new Vector2(
                startPos.x + size.x *
                Mathf.Sin(Mathf.FloorToInt(multiplier.x) * Time.timeSinceLevelLoad + Mathf.PI / 2),
                startPos.y + size.y * Mathf.Sin(Mathf.CeilToInt(multiplier.y) * Time.timeSinceLevelLoad)
            );
        }
    }
}
