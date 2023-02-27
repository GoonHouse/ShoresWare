using System.Collections;
using System.Collections.Generic;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class AlphaTheDog : MonoBehaviour {
    public int engageIndex;

    public List<AudioClip> blips;
    public AudioSource blipper;
    public AudioSource blipGasm;

    public PlayableDirector _playableDirector;

    public TimelineAsset blipGasmo;

    public UnityEvent onWin;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Engage();
        }
    }

    void Win() {
        onWin.Invoke();
        Debug.Log("you win!");
        blipGasm.Play();
        _playableDirector.Play(blipGasmo);
        FindObjectOfType<Elevator>().Win();
    }

    private void Engage() {
        if (engageIndex <= blips.Count - 1) {
            _playableDirector.Play();
        }
    }

    public void Blip() {
        blipper.PlayOneShot(blips[engageIndex]);
        engageIndex++;
        
        if (engageIndex >= blips.Count) {
            Win();
        }
    }
}
