using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class PSBoi : MonoBehaviour {
    private bool alive = true;
    public float moveSpeed;

    public AudioSource getHair;
    public AudioSource getPuddle;
    public Image image;

    public Sprite deadSprite;
    public UnityEvent onWin;
    public UnityEvent onLose;

    // Update is called once per frame
    void Update() {
        if (!alive) return;
        var moveVector = (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) * Time.deltaTime * moveSpeed;
        transform.position += moveVector;
    }

    void GetTouched(GameObject touchedBy) {
        if (!alive) return;
        if (touchedBy.name == "Hair") {
            Debug.Log("touched it");
            touchedBy.SetActive(false);
            getHair.Play();

            var touchables = FindObjectsOfType<Touchable>().Where((touchable, i) => touchable.gameObject.name == "Hair");
            if (!touchables.Any()) {
                Win();
            }
        } else if (touchedBy.name == "Puddle") {
            Lose();
        }
    }

    void Win() {
        onWin.Invoke();
        FindObjectOfType<Elevator>().Win();
    }

    void Lose() {
        image.sprite = deadSprite;
        getPuddle.Play();
        alive = false;
        onLose.Invoke();
        FindObjectOfType<Elevator>().Lose();
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("PSBoi: touched a " + other.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("PSBoi: got touched");
    }
}
