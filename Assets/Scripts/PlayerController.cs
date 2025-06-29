using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("移動速度"), SerializeField] private float _moveSpeed = 5f;
	[Header("上昇・下降速度"), SerializeField] private float _verticalSpeed = 3f;
	[Header("回転のスムーズさ"), SerializeField] private float _turnSmoothTime = 0.1f;

	[Header("潜る角度の制限"), SerializeField] private float _maxPitchAngle = 45f;
	[Header("上下回転のスムーズさ"), SerializeField] private float _pitchSmoothTime = 0.2f;

	[Header("移動できる高さの制限")]
	[SerializeField] private float _minY = -20f;
	[SerializeField] private float _maxY = 5f;

	[Header("海面・霧のオブジェクト")]
	[SerializeField] private GameObject surfaceFloor;
	[SerializeField] private GameObject surfaceFog;

	[SerializeField] private FogController fogController;

	private Vector2 _inputHorizontal;
	private float _inputVertical;

	private CharacterController _controller;
	private Transform _transform;
	private float _turnVelocity;
	private float _currentPitch = 0f;
	private float _pitchVelocity = 0f;

	private bool isUnderwater = false;

	public void OnMove(InputAction.CallbackContext context)
	{
		_inputHorizontal = context.ReadValue<Vector2>();
	}

	public void OnSwimVertical(InputAction.CallbackContext context)
	{
		_inputVertical = context.ReadValue<float>();
	}

	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_transform = transform;

		// カーソルロック
		Cursor.lockState = CursorLockMode.Locked;

		// カーソル非表示
		Cursor.visible = false;
	}

	private void Update()
	{
		Transform cam = Camera.main.transform;

		// カメラ基準
		Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 camRight = cam.right;

		Vector3 inputDir = new Vector3(_inputHorizontal.x, 0f, _inputHorizontal.y).normalized;
		Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;
		Vector3 verticalMove = Vector3.up * _inputVertical;

		Vector3 move = (moveDir * _moveSpeed + verticalMove * _verticalSpeed) * Time.deltaTime;
		_controller.Move(move);

		// 高さ制限
		Vector3 pos = _transform.position;
		pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
		_transform.position = pos;

		// 水平回転（Yaw）
		if (moveDir.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothTime);
			_transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, angle, 0f);
		}

		// 上下回転（Pitch） - 前進時のみ反映
		float forwardAmount = Vector3.Dot(moveDir.normalized, _transform.forward);
		float targetPitch = -_inputVertical * _maxPitchAngle * Mathf.Clamp01(forwardAmount);
		_currentPitch = Mathf.SmoothDamp(_currentPitch, targetPitch, ref _pitchVelocity, _pitchSmoothTime);

		// 回転にPitch反映
		float currentYaw = _transform.eulerAngles.y;
		_transform.rotation = Quaternion.Euler(_currentPitch, currentYaw, 0f);

		// 海面の追従
		if (surfaceFloor.activeSelf)
		{
			Vector3 floorPos = surfaceFloor.transform.position;
			floorPos.x = _transform.position.x;
			floorPos.z = _transform.position.z;
			surfaceFloor.transform.position = floorPos;
		}

		// 水中/水上判定と効果切り替え
		bool nowUnderwater = WaterManager.Instance.IsUnderwater(_transform.position);
		if (nowUnderwater != isUnderwater)
		{
			isUnderwater = nowUnderwater;

			if (isUnderwater)
			{
				AudioManager.Instance.PlayUnderwaterAudio();
				surfaceFog.SetActive(false);
				fogController.FadeOut();
			}
			else
			{
				AudioManager.Instance.PlayNormalAudio();
				surfaceFog.SetActive(true);
				fogController.FadeIn();
			}
		}
	}
}
