using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // @TODO: definitely make this a fixedupdate please for the love of god
    void Update() {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }
}
