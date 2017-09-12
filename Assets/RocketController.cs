using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour {

    public Rigidbody rb;

    private void Start () {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate () {
        if (Input.GetMouseButtonDown(0)) {
            rb.AddForce(transform.forward * 2000);
            rb.AddForce(Vector3.left * 100);
        }

        Vector3 target = Vector3.zero - transform.position;
        rb.AddForce(target.normalized * 100);
    }

}
