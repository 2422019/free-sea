using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaCurrentZone : MonoBehaviour
{
	public Vector3 pushDirection = Vector3.back;  // �����߂������i��F���ɖ߂��j
	public float pushSpeed = 3f;

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
	}
}


