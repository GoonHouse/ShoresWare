using System.Collections;
using System.Collections.Generic;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class Clipper : MonoBehaviour {
    
    public Image tooth;
    public List<Sprite> toothSprites;
    public int toothIndex;
    public Transform arm;
    public AudioSource clip;
    
    public float downZ = -9.319f;
    public float upZ = 15.95f;

    public UnityEvent onWin;
    
    // Start is called before the first frame update
    void Start() {
        toothIndex = toothSprites.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Clip();
        }
    }

    private void Clip() {
        if (toothIndex > 0) {
            toothIndex--;
            tooth.sprite = toothSprites[toothIndex];
            clip.Play();
            StartCoroutine(Arm());
            
            if (toothIndex <= 0) {
                onWin.Invoke();
                Debug.Log("you win!");
                FindObjectOfType<Elevator>().Win();
            }
        }
    }

    IEnumerator Arm() {
        arm.Rotate(new Vector3(0, 0, downZ));
        yield return new WaitForSeconds(0.1f);
        arm.Rotate(new Vector3(0, 0, upZ));
    }
}
