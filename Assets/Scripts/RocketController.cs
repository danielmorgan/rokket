using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(InfluencedByPlanetaryGravity))]
public class RocketController : MonoBehaviour {

    Rigidbody body;
    LineRenderer line;
    InfluencedByPlanetaryGravity g;

    float lineOffsetY;
    Vector3 lineEnd;

    public float maxThrust = 100f;
    public float thrustSetRate = 50f;
    public float thrustMultiplier = 100f;
    float thrust = 0f;

    void Start () {
        body = GetComponent<Rigidbody>();
        line = GetComponentInChildren<LineRenderer>();
        g = GetComponent<InfluencedByPlanetaryGravity>();

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

        int sampleCount = Mathf.Clamp((int) (thrust), 1, 200);
        line.positionCount = sampleCount;
        Vector3[] samples = new Vector3[sampleCount];
        samples[0] = line.GetPosition(0);
        for (int x = 1; x < sampleCount; x++) {
            float y = HeightAtX(x);
            //Debug.Log(x + "," + y);
            samples[x] = new Vector3(y / 5, x + lineOffsetY, 0f);
        }
        line.SetPositions(samples);

        //lineEnd = line.GetPosition(1);
        //lineEnd.y = lineOffsetY + (thrust * 0.1f);
        //line.SetPosition(1, lineEnd);
    }

    void FireEngine (float thrust) {
        body.AddForce(transform.forward * thrust * thrustMultiplier);
    }

    float HeightAtX (int x) {
        float a = transform.rotation.x * Mathf.Deg2Rad;
        Vector3 v = thrust * transform.forward;

        return (x * Mathf.Tan(a)) -
            (
                (g.gravity * Mathf.Pow(x, 2))
                /
                (2 * Mathf.Pow(v.y * Mathf.Cos(a), 2))
            );
    }
}
