using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPower : MonoBehaviour
{
	[SerializeField] float buoyancyForce = 10f;
	[SerializeField] float waterHeight = 0f;
	private Rigidbody rb;

	void Awake() => rb = GetComponent<Rigidbody>();

	void FixedUpdate()
	{
		if (transform.position.y < waterHeight)
		{
			float depth = waterHeight - transform.position.y;
			rb.AddForce(Vector3.up * buoyancyForce * depth);
		}
	}
}

