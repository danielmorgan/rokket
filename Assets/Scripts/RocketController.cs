using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RocketController : MonoBehaviour {

	public GameObject rocket;
	[Range(1,1000)]
	public float thrustMultiplier = 100f;
	[Range(1,100)]
	public float thrust = 10f;
	[Range(-90,90)]
	public float angle = 45f;
	[Range(2, 100)]
	public int resolution = 20;

	LineRenderer line;
	float a; // angle in radians
	float g; // magnitude of gravity (9.82)

    void Awake () {
        line = GetComponent<LineRenderer>();
		a = Mathf.Deg2Rad * angle;
		g = Mathf.Abs(Physics.gravity.y);
    }

	void OnValidate () {
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

		Vector3 sample = new Vector3(x, y);

		sample = WrapAroundPlanet(sample);

		return sample;
	}

	Vector3 WrapAroundPlanet (Vector3 p) {
		float radius = 20.19503f;
		float circumference = 2 * Mathf.PI * radius;
		float theta = -(p.x / circumference) * Mathf.PI;

		float x = radius * Mathf.Cos(theta);
		float y = radius * Mathf.Sin(theta);

		return new Vector3(x, y);
	}

    void FixedUpdate () {
		a = Mathf.Deg2Rad * angle;

		RenderTrajectoryArc();

		if (Input.GetMouseButtonDown(0)) {
			Vector3 impulse = new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * thrust;
			rocket.GetComponent<Rigidbody>().AddForce(impulse * thrustMultiplier);
		}
    }
}
