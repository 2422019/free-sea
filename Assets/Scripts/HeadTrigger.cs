using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
	[Header("�w�f�B���O���ɉ������")]
	public float headingForce = 5f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if (rb != null)
			{
				// �N�W���̑O�������ɗ͂�������i�w�f�B���O�j
				Vector3 headingDirection = transform.forward;
				rb.AddForce(headingDirection * headingForce, ForceMode.Impulse);
			}
		}
	}
}
