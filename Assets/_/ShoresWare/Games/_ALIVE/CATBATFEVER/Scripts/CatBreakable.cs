using System.Collections.Generic;
using BeeCore.Extensions;
using ShoresWare.Core;
using UnityEngine;

namespace ShoresWare.Games.CATBATFEVER {
    public class CatBreakable : MicroGameObject
    {
        public Vector2 gibsToSpawn = new Vector2(1.0f, 3.0f);
        public Vector2 gibExplodeForce = new Vector2(1.0f, 3.0f);
        public List<GameObject> gibs = new List<GameObject>();
        public List<AudioClip> breakSounds = new List<AudioClip>();

        private BoomBox _boomBox;

        private void Start() {
            _boomBox = FindObjectOfType<BoomBox>();
        }

        public void Break() {
            var toSpawn = Mathf.RoundToInt(gibsToSpawn.Random());

            for (int i = 0; i < toSpawn; i++) {
                var gib = Instantiate(gibs.Random(), transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                var _rb = gib.GetComponent<Rigidbody2D>();
                _rb.AddRelativeForce(Vector2.up * gibExplodeForce.Random(), ForceMode2D.Impulse);
            }

            _boomBox.PlaySound(breakSounds.Random());
            
            Destroy(gameObject);
        }
        
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.name == "Floor") {
                Break();
            }
        }
    }
}
