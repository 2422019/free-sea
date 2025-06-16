using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaCurrentZone : MonoBehaviour
{
	[Header("押し返す方向とスピード")]
	public Vector3 pushDirection = Vector3.back;  // 押し戻す方向（例：奥に戻す）
	[SerializeField] private float pushSpeed = 5f;

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CharacterController controller = other.GetComponent<CharacterController>();
			if (controller != null)
			{
				Vector3 push = pushDirection.normalized * pushSpeed * Time.deltaTime;
				controller.Move(push);
			}
		}
		if(other.CompareTag("Ball"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if(rb != null)
			{
				Vector3 force = pushDirection.normalized * pushSpeed;
				rb.AddForce(force, ForceMode.Force);
			}
		}
	}
}


