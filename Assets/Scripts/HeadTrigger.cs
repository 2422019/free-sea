using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
	[Header("�w�f�B���O���ɉ������")]
	[SerializeField] private float headingForce = 5f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball"))
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if (rb != null)
			{
				// Y��������x���Z�b�g���Ă��璵�˂�����
				rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

				// �N�W���̑O�{��Ɍ����ė͂�������
				Vector3 headingDirection = (transform.forward + Vector3.up * 0.5f).normalized;
				rb.AddForce(headingDirection * headingForce, ForceMode.Impulse);

			}
		}
	}
}
