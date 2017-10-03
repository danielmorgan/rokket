using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RocketController : MonoBehaviour {

	public GameObject rocket;
	public GameObject planet;
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
		// @todo get radius from planet object
		float radius = 23f;
		float circumference = 2 * Mathf.PI * radius;
		float initialRadians = (Mathf.PI / 2);
		float travelledRadians = -(p.x / circumference);
		float theta = initialRadians + travelledRadians;

		// How far up is the point?
		float height = radius + p.y;

		// Convert from polar to cartesian coords
		float x, y;
		x = height * Mathf.Cos(theta);
		y = height * Mathf.Sin(theta);

		// Pull it down due to gravity
		Vector3 separation = planet.transform.position - new Vector3(x, y);
		float magnitude = Mathf.Clamp(separation.sqrMagnitude, 0f, 750f);
		float gravityAtCurrentRange = (planet.GetComponent<Rigidbody>().mass * g) / magnitude;
		float gravity = rocket.GetComponent<Rigidbody>().mass * gravityAtCurrentRange;
		Vector3 fall = separation.normalized * gravity / 10f;

		// Re-convert but with new height
		Vector3 p2 = new Vector3(x, y);
		p2 += fall;
		return p2;
	}

    void FixedUpdate () {
		a = angle * Mathf.Deg2Rad;

		RenderTrajectoryArc();

		if (Input.GetMouseButtonDown(0)) {
			Vector3 impulse = new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * thrust;
			rocket.GetComponent<Rigidbody>().AddForce(impulse * thrustMultiplier);
		}
    }
}
