using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woolie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetTouched(GameObject touchedBy) {
        gameObject.SetActive(false);
    }
}
