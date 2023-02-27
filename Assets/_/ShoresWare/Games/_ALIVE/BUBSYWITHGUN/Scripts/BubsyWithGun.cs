using System.Linq;
using ShoresWare.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Input = UnityEngine.Input;

namespace ShoresWare.Games.BUBSYWITHGUN {
    public class BubsyWithGun : MicroGameObject {
        public int ammo;
        public int maxAmmo = 6;

        public AudioSource noAmmoSource;

        public GameObject bullet;
        public Transform bulletSpawnPoint;
        public UnityEvent onWin;
        public UnityEvent onLose;

        public TextMeshProUGUI hud;

        private void Start() {
            ammo = maxAmmo;
            UpdateHUD();
        }

        // Update is called once per frame
        void Update() {
            var woolies = FindObjectsOfType<Woolie>().Where((woolie, i) => woolie.gameObject.name.StartsWith("Woolie") );
            if (!woolies.Any()) {
                Win();
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Shoot();
            }
        }

        void UpdateHUD() {
            hud.text = $"Ammo: {ammo}/{maxAmmo}; Bubsy: yes";
        }
    
        void Shoot() {
            if (ammo > 0) {
                ammo--;
                UpdateHUD();
                var newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation, bulletSpawnPoint);
            } else {
                noAmmoSource.Play();
                Lose();
            }
        }
    
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.name == "Bullet") {
                Lose();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name == "Bullet") {
                Lose();
            }
        }
    }
}
