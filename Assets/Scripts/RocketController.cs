using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class RocketController : MonoBehaviour {

    Rigidbody body;
    LineRenderer line;
    public float thrustAmount = 2500f;
    bool dragging = false;

    void Start () {
        body = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
    }

    void FixedUpdate () {
        if (Input.GetMouseButton(0)) {
            // Going back, check vector3 is only going up in world context
            Vector3 target = Vector3.up * Input.GetAxis("Mouse Y");
            line.SetPosition(1, target);
            Debug.Log(Input.GetAxis("Mouse Y"));
        }
    }

    void FireEngine () {
        body.AddForce(transform.forward * thrustAmount);
    }
}
