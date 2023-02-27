using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasedTurkey : MonoBehaviour {

    public float upTime;
    private bool isUpping;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("yo");
        
        if (other.gameObject.name == "TurkeyUp") {
            StartCoroutine(UpThisTurkey());
        }
    }

    IEnumerator UpThisTurkey() {
        if(isUpping) yield break;

        isUpping = true;
        var x = 0.0f;
        while (x < upTime) {
            var transformLocalPosition = transform.localPosition;
            transformLocalPosition.x += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        x = 0.0f;
        while (x < upTime) {
            var transformLocalPosition = transform.localPosition;
            transformLocalPosition.x -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isUpping = false;
    }
}
