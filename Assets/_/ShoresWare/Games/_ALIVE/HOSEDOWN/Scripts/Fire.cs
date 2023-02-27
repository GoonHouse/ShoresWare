using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public int startSize;
    public int size;
    
    
    // Start is called before the first frame update
    void Start() {
        startSize = Random.Range(2, 7);
        size = startSize;
        transform.localScale = size * Vector3.one;
    }

    void Shrink(GameObject water) {
        size--;
        transform.localScale = size * Vector3.one;
        if (size == 0) {
            gameObject.SetActive(false);                
        }
        Destroy(water);
    }
    
    public void GetTouched(GameObject touchedBy) {
        if (touchedBy.gameObject.name.StartsWith("Water")) {
            Shrink(touchedBy);
        }
    }
}
