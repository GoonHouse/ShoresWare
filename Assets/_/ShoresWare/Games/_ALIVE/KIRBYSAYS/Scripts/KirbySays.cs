using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Events;
using Input = UnityEngine.Input;

public class KirbySays : MonoBehaviour {
    
    public List<KeyCode> phrase;
    private int phraseIndex = 0;
    public List<GameObject> letterGraphics;

    public AudioSource wrong;

    public UnityEvent onWin;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(keyCode)) {
                if (phrase[phraseIndex] == keyCode) {
                    RightKey();
                } else {
                    WrongKey();
                }
            }
        }
    }

    private void RightKey() {
        letterGraphics[phraseIndex].SetActive(true);
        phraseIndex++;

        if (phraseIndex == phrase.Count) {
            onWin.Invoke();
            FindObjectOfType<Elevator>().Win();
        } 
    }

    private void WrongKey() {
        phraseIndex = 0;
        foreach (var letterGraphic in letterGraphics) {
            letterGraphic.SetActive(false);
        }
        wrong.Play();
    }
}
