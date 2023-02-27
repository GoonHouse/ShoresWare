using UnityEngine;

namespace ShoresWare.Games.CATBATFEVER {
    public class Wobble : MonoBehaviour {
        public float extent;
        public float speed;

        // Update is called once per frame
        void Update() {
            var euler = transform.localRotation.eulerAngles;
            euler.z = Mathf.PingPong(Time.time * speed, extent * 2) - extent;
            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
