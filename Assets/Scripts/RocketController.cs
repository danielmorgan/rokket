using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour {

    Rigidbody body;
    public GameObject planet;
    public float thrustAmount = 2000f;
    public float gravityFactor = 1f;

    void Start () {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        ApplyGravity();

        if (Input.GetMouseButtonDown(0)) {
            FireEngine();
        }
    }

    void ApplyGravity () {
        Vector3 target = planet.transform.position - transform.position;
        float currentInverseSquareMagnitude = (planet.transform.position - transform.position).sqrMagnitude;
        float gravityAtCurrentRange = (planet.GetComponent<Rigidbody>().mass * gravityFactor) / currentInverseSquareMagnitude;
        float magnitudeToApply = body.mass * gravityAtCurrentRange;

        body.AddForce(target.normalized * magnitudeToApply);
    }

    void FireEngine () {
        body.AddForce(transform.forward * thrustAmount);
    }
}
