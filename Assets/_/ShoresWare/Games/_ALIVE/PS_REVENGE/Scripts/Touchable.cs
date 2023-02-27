using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetTouched() {
        // i don't care, actually
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Touchable: touched a " + other.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("got touched");
        other.BroadcastMessage("GetTouched", gameObject);
    }
}
