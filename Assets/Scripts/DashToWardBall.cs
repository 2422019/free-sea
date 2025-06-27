using UnityEngine;

public class DashTowardBall : MonoBehaviour
{
	[SerializeField] private Transform targetBall;    // 目標ボール
	[SerializeField] private float dashSpeed = 20f;   // ダッシュ速度
	[SerializeField] private float dashDuration = 1f; // ダッシュ時間
	[SerializeField] private float dashCooldown = 3f; // クールタイム

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
		// クールタイム更新
		if (cooldownTimer > 0f)
			cooldownTimer -= Time.deltaTime;

		if (isDashing)
		{
			// ダッシュ継続
			dashTimer += Time.deltaTime;
			if (dashTimer < dashDuration)
			{
				controller.Move(dashDirection * dashSpeed * Time.deltaTime);
			}
			else
			{
				// ダッシュ終了
				isDashing = false;
				cooldownTimer = dashCooldown;
			}
		}
		else
		{
			// ダッシュ開始条件
			if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f && targetBall != null)
			{
				Vector3 toBall = targetBall.position - transform.position;

				// ボールが自分より上にあるなら上下成分も含める
				if (toBall.y > 0f)
				{
					dashDirection = toBall.normalized;
				}
				else
				{
					// 水平面上のみ
					dashDirection = new Vector3(toBall.x, 0f, toBall.z).normalized;
				}

				isDashing = true;
				dashTimer = 0f;

				// ダッシュ方向に向きを補正
				transform.rotation = Quaternion.LookRotation(new Vector3(dashDirection.x, 0f, dashDirection.z));
			}
		}
	}
}

