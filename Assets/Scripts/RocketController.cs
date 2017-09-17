using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour {

    Rigidbody body;
    LineRenderer line;
    float lineOffsetY;

	public float thrustMultiplier = 100f;
	public float thrust = 10f;
	public float angle = 45f;
	public int resolution = 20;

	float a; // angle in radians
	float g; // magnitude of gravity (9.82)

    void Awake () {
        body = GetComponent<Rigidbody>();
        line = GetComponentInChildren<LineRenderer>();
        lineOffsetY = line.GetPosition(0).y;
		g = Mathf.Abs(Physics.gravity.y);
    }

	void OnValidate () {
		thrust = Mathf.Clamp(thrust, 1, 100);
		angle = Mathf.Clamp(angle, -90, 90);
		resolution = Mathf.Clamp(resolution, 0, 50);

		if (line != null) {
			RenderTrajectoryArc();
		}
	}

	void Start () {
		RenderTrajectoryArc();
	}

	void RenderTrajectoryArc () {
		line.positionCount = resolution + 1;
		line.SetPositions(CalculateArcArray());
	}

	Vector3[] CalculateArcArray () {
		Vector3[] arcArray = new Vector3[resolution + 1];
		float maxDistance = (thrust * thrust * Mathf.Sin(2 * a)) / g;

		for (int i = 0; i < resolution + 1; i++) {
			float t = (float)i / (float)resolution;
			arcArray[i] = SampleArc(t, maxDistance);
		}

		return arcArray;
	}

	Vector3 SampleArc (float t, float maxDistance) {
		float x = t * maxDistance;
		float y = x * Mathf.Tan(a) - ((g * x * x) / (2 * thrust * thrust * Mathf.Cos(a) * Mathf.Cos(a)));

		return new Vector3(x, y + lineOffsetY);
	}

    void Update () {
		a = Mathf.Deg2Rad * angle;

		RenderTrajectoryArc();

		if (Input.GetMouseButtonDown(0)) {
			Vector3 impulse = new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * thrust;
			body.AddForce(impulse * thrustMultiplier);
        }
    }
}
