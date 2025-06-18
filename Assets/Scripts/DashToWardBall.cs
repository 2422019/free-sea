using UnityEngine;

public class DashTowardBall : MonoBehaviour
{
	[SerializeField] private Transform targetBall;        // �ڕW�{�[��
	[SerializeField] private float dashSpeed = 20f;       // �_�b�V�����x
	[SerializeField] private float dashDuration = 1f;     // �_�b�V������
	[SerializeField] private float dashCooldown = 3f;     // �N�[���^�C��

	private bool isDashing = false;
	private float dashTimer = 0f;
	private float cooldownTimer = 0f;

	private CharacterController controller;
	private Vector3 dashDirection;

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		// �N�[���^�C���X�V
		if (cooldownTimer > 0f)
			cooldownTimer -= Time.deltaTime;

		// �_�b�V����
		if (isDashing)
		{
			dashTimer += Time.deltaTime;
			if (dashTimer < dashDuration)
			{
				controller.Move(dashDirection * dashSpeed * Time.deltaTime);
			}
			else
			{
				isDashing = false;
				cooldownTimer = dashCooldown;
			}
		}
		else
		{
			// �X�y�[�X�L�[�Ń_�b�V���J�n
			if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f && targetBall != null)
			{
				dashDirection = (targetBall.position - transform.position).normalized;
				//dashDirection.y = 0f; // ���������̂�
				isDashing = true;
				dashTimer = 0f;

				// �N�W���̌�������������
				transform.rotation = Quaternion.LookRotation(dashDirection);
			}
		}
	}
}
