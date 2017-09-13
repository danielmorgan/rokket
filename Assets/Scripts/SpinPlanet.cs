using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpinPlanet : MonoBehaviour {

    Rigidbody body;
    public float speed;

    void Start () {
        body = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate () {
        Vector3 rotation = (Vector3.up).normalized * speed;
        Quaternion deltaRotation = Quaternion.Euler(rotation);
        body.MoveRotation(body.rotation * deltaRotation);
    }
}
