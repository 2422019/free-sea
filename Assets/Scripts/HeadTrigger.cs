using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
	[Header("ヘディング時に加える力")]
	[SerializeField] private float headingForce = 5f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if (rb != null)
			{
				// Y方向を一度リセットしてから跳ねさせる
				rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

				// クジラの前＋上に向けて力を加える
				Vector3 headingDirection = (transform.forward + Vector3.up * 0.5f).normalized;
				rb.AddForce(headingDirection * headingForce, ForceMode.Impulse);

			}
		}
	}
}
