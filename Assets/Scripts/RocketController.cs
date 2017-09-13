using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour {

    Rigidbody body;

    public LineRenderer line;
    float lineOffsetY;
    Vector3 lineEnd;

    public float maxThrust = 100f;
    public float thrustSetRate = 100f;
    public float thrustMultiplier = 100f;
    float thrust = 0f;

    void Start () {
        body = GetComponent<Rigidbody>();
        line = GetComponentInChildren<LineRenderer>();
        lineOffsetY = line.GetPosition(0).y;
    }

    void Update () {
        if (Input.GetMouseButton(0)) {
            float thrustToApply = thrustSetRate * Time.deltaTime;
            thrust = Mathf.Clamp(thrust += thrustToApply, 0f, maxThrust);
        } else {
            FireEngine(thrust);
            thrust = 0f;
        }

        lineEnd = line.GetPosition(1);
        lineEnd.y = lineOffsetY + (thrust * 0.1f);
        line.SetPosition(1, lineEnd);
    }

    void FireEngine (float thrust) {
        body.AddForce(transform.forward * thrust * thrustMultiplier);
    }
}
