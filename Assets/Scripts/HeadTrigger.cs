using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
	[Header("ヘディング時に加える力")]
	public float headingForce = 5f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if (rb != null)
			{
				// クジラの前方方向に力を加える（ヘディング）
				Vector3 headingDirection = transform.forward;
				rb.AddForce(headingDirection * headingForce, ForceMode.Impulse);
			}
		}
	}
}
