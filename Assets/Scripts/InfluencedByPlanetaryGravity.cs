using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InfluencedByPlanetaryGravity : MonoBehaviour {

    Rigidbody body;
    public GameObject planet;
    public float gravitationalConstant = 9.807f;
    float planetMass;

    void Start () {
        body = GetComponent<Rigidbody>();
        planetMass = planet.GetComponent<Rigidbody>().mass;
    }

	void FixedUpdate () {
        Vector3 separation = planet.transform.position - transform.position;
        float magnitude = Mathf.Clamp(separation.sqrMagnitude, 0f, 750f);
        float gravityAtCurrentRange = (planetMass * gravitationalConstant) / magnitude;
        float magnitudeToApply = body.mass * gravityAtCurrentRange;

        body.AddForce(separation.normalized * magnitudeToApply);
	}
}
