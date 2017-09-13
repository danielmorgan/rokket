using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour {

    Rigidbody body;
    public float thrustAmount = 2500f;

    void Start () {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if (Input.GetMouseButtonDown(0)) {
            FireEngine();
        }
    }

    void FireEngine () {
        body.AddForce(transform.forward * thrustAmount);
    }
}
