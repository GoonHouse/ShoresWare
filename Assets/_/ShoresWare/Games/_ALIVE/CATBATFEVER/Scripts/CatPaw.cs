using System.Linq;
using ShoresWare.Core;
using UnityEngine;
using Input = UnityEngine.Input;

namespace ShoresWare.Games.CATBATFEVER {
    public class CatPaw : MicroGameObject {
        public Vector3 offset;

        private GameObject _prompt;
        private SpriteRenderer _renderer;
        private bool _engaged;
        private float _circleSweep = 0.1f;
        private Vector3 _startOffset;
        private Camera _camera;
        private Rigidbody2D _rb;
        
        private void Start() {
            _camera = Camera.main;
            _startOffset = transform.position - _camera.transform.position;
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _prompt = transform.Find("Prompt").gameObject;
            Disengage();
        }

        private void Disengage() {
            _engaged = false;
            _renderer.color = Color.black;
            _prompt.SetActive(true);
            Cursor.visible = true;
            // @TODO: really, we want to release the cursor in this case
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Engage() {
            _engaged = true;
            _renderer.color = Color.white;
            _prompt.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update() {
            if (_engaged) {
                var worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);

                _rb.MovePosition(worldPoint - offset + _startOffset);
            } else {
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                var mouse2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D circleCast = Physics2D.CircleCast(mouse2D, _circleSweep, Vector2.zero);
                Debug.DrawLine(mousePos, mousePos + Vector3.one * _circleSweep);
                if (!circleCast) return;
                
                if (circleCast.transform.gameObject == gameObject) {
                    Engage();
                }
            }

            var catBreakables = FindObjectsOfType<CatBreakable>();
            if (!catBreakables.Any()) {
                FindObjectOfType<Elevator>().Win();
            }
        }

        private void OnDestroy() {
            Disengage();
        }
    }
}
