using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InfluencedByPlanetaryGravity : MonoBehaviour {

    Rigidbody body;
    public GameObject planet;
    public float gravity = 9.807f;
    float planetMass;

    void Start () {
        body = GetComponent<Rigidbody>();
        planetMass = planet.GetComponent<Rigidbody>().mass;
    }

	void FixedUpdate () {
        Vector3 target = planet.transform.position - transform.position;
        float currentInverseSquareMagnitude = (planet.transform.position - transform.position).sqrMagnitude;
        float gravityAtCurrentRange = (planetMass * gravity) / currentInverseSquareMagnitude;
        float magnitudeToApply = body.mass * gravityAtCurrentRange;

        body.AddForce(target.normalized * magnitudeToApply);
	}
}
