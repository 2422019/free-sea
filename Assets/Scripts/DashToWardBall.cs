using UnityEngine;

public class DashTowardBall : MonoBehaviour
{
	[SerializeField] private Transform targetBall;    // �ڕW�{�[��
	[SerializeField] private float dashSpeed = 20f;   // �_�b�V�����x
	[SerializeField] private float dashDuration = 1f; // �_�b�V������
	[SerializeField] private float dashCooldown = 3f; // �N�[���^�C��

	private bool isDashing = false;
	private float dashTimer = 0f;
	private float cooldownTimer = 0f;

	private CharacterController controller;
	private Vector3 dashDirection;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		// �N�[���^�C���X�V
		if (cooldownTimer > 0f)
			cooldownTimer -= Time.deltaTime;

		if (isDashing)
		{
			// �_�b�V���p��
			dashTimer += Time.deltaTime;
			if (dashTimer < dashDuration)
			{
				controller.Move(dashDirection * dashSpeed * Time.deltaTime);
			}
			else
			{
				// �_�b�V���I��
				isDashing = false;
				cooldownTimer = dashCooldown;
			}
		}
		else
		{
			// �_�b�V���J�n����
			if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f && targetBall != null)
			{
				Vector3 toBall = targetBall.position - transform.position;

				// �{�[������������ɂ���Ȃ�㉺�������܂߂�
				if (toBall.y > 0f)
				{
					dashDirection = toBall.normalized;
				}
				else
				{
					// �����ʏ�̂�
					dashDirection = new Vector3(toBall.x, 0f, toBall.z).normalized;
				}

				isDashing = true;
				dashTimer = 0f;

				// �_�b�V�������Ɍ�����␳
				transform.rotation = Quaternion.LookRotation(new Vector3(dashDirection.x, 0f, dashDirection.z));
			}
		}
	}
}

