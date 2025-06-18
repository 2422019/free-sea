using UnityEngine;

public class DashTowardBall : MonoBehaviour
{
	[SerializeField] private Transform targetBall;        // 目標ボール
	[SerializeField] private float dashSpeed = 20f;       // ダッシュ速度
	[SerializeField] private float dashDuration = 1f;     // ダッシュ時間
	[SerializeField] private float dashCooldown = 3f;     // クールタイム

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
		// クールタイム更新
		if (cooldownTimer > 0f)
			cooldownTimer -= Time.deltaTime;

		// ダッシュ中
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
			// スペースキーでダッシュ開始
			if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f && targetBall != null)
			{
				dashDirection = (targetBall.position - transform.position).normalized;
				//dashDirection.y = 0f; // 水平方向のみ
				isDashing = true;
				dashTimer = 0f;

				// クジラの向きを向かせる
				transform.rotation = Quaternion.LookRotation(dashDirection);
			}
		}
	}
}
