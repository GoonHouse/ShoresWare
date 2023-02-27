using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Events;
using Input = UnityEngine.Input;

public class HoseLad : MonoBehaviour {

    public float rotateSpeed;
    public Transform spawnPoint;

    public GameObject waterPrefab;

    public float rateOfSpawn = 0.25f;
    public float spawnTimer = 0.0f;
    
    public UnityEvent onWin;
    public UnityEvent onLose;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.A)) {
            var transformRotation = transform.rotation.eulerAngles;
            transformRotation.z += (rotateSpeed * Time.deltaTime);

            var quaternion = transform.rotation;
            quaternion.eulerAngles = transformRotation;
            transform.rotation = quaternion;
        }
        
        if (Input.GetKey(KeyCode.D)) {
            var transformRotation = transform.rotation.eulerAngles;
            transformRotation.z -= (rotateSpeed * Time.deltaTime);
            
            var quaternion = transform.rotation;
            quaternion.eulerAngles = transformRotation;
            transform.rotation = quaternion;
        }
        
        var fires = FindObjectsOfType<Fire>().Where((fire, i) => fire.gameObject.name.StartsWith("Fire") );
        if (!fires.Any()) {
            Win();
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= rateOfSpawn) {
            spawnTimer -= rateOfSpawn;
            Spawn();
        }
    }

    void Spawn() {
        var newDrop = Instantiate(waterPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        var oldScale = newDrop.transform.localScale;
        
        newDrop.transform.SetParent(GetComponentInParent<Canvas>().transform, true);
        newDrop.transform.localScale = oldScale;
    }

    void Win() {
        onWin.Invoke();
        FindObjectOfType<Elevator>().Win();
    }

    void Lose() {
        onLose.Invoke();
        FindObjectOfType<Elevator>().Lose();
    }
}
