using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("移動速度"), SerializeField] private float _moveSpeed = 5f;
	[Header("上昇・下降速度"), SerializeField] private float _verticalSpeed = 3f;
	[Header("回転のスムーズさ"), SerializeField] private float _turnSmoothTime = 0.1f;

	private Vector2 _inputHorizontal; // 左スティック用（XZ移動）
	private float _inputVertical;     // 上下移動用（例：A/Z, トリガー）

	private CharacterController _controller;
	private Transform _transform;
	private float _turnVelocity;

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
	}

	private void Update()
	{
		Vector3 inputDir = new Vector3(_inputHorizontal.x, 0f, _inputHorizontal.y).normalized;
		Vector3 verticalMove = Vector3.up * _inputVertical;

		Vector3 move = (inputDir * _moveSpeed + verticalMove * _verticalSpeed) * Time.deltaTime;
		_controller.Move(move);

		// 向きを滑らかに変更（XZ方向のみ）
		if (inputDir.magnitude > 0.1f)
		{
			float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothTime);
			_transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}

		// クジラが水の中にいるかどうかでAudioを切り替える
		bool nowUnderwater = WaterManager.Instance.IsUnderwater(transform.position);

		if (nowUnderwater != isUnderwater)
		{
			isUnderwater = nowUnderwater;

			if (isUnderwater)
				AudioManager.Instance.PlayUnderwaterAudio();
			else
				AudioManager.Instance.PlayNormalAudio();
		}
	}
}

/*
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("移動の速さ"), SerializeField]
	private float _speed = 3;

	[Header("ジャンプする瞬間の速さ"), SerializeField]
	private float _jumpSpeed = 7;

	[Header("重力加速度"), SerializeField]
	private float _gravity = 15;

	[Header("落下時の速さ制限（Infinityで無制限）"), SerializeField]
	private float _fallSpeed = 10;

	[Header("落下の初速"), SerializeField]
	private float _initFallSpeed = 2;

	private Transform _transform;
	private CharacterController _characterController;

	private Vector2 _inputMove;
	private float _verticalVelocity;
	private float _turnVelocity;
	private bool _isGroundedPrev;

	public void OnMove(InputAction.CallbackContext context)
	{
		// 入力値を保持しておく
		_inputMove = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		// ボタンが押された瞬間かつ着地している時だけ処理
		if (!context.performed || !_characterController.isGrounded) return;

		// 鉛直上向きに速度を与える
		_verticalVelocity = _jumpSpeed;
	}

	private void Awake()
	{
		_transform = transform;
		_characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		bool isGrounded = _characterController.isGrounded;

		if (isGrounded && !_isGroundedPrev)
		{
			// 着地する瞬間に落下の初速を指定しておく
			_verticalVelocity = -_initFallSpeed;
		}
		else if (!isGrounded)
		{
			// 空中にいるときは、下向きに重力加速度を与えて落下させる
			_verticalVelocity -= _gravity * Time.deltaTime;

			// 落下する速さ以上にならないように補正
			if (_verticalVelocity < -_fallSpeed)
				_verticalVelocity = -_fallSpeed;
		}

		_isGroundedPrev = isGrounded;

		// 操作入力と鉛直方向速度から、現在速度を計算
		Vector3 moveVelocity = new Vector3(
			_inputMove.x * _speed,
			_verticalVelocity,
			_inputMove.y * _speed
		);
		// 現在フレームの移動量を移動速度から計算
		Vector3 moveDelta = moveVelocity * Time.deltaTime;

		// CharacterControllerに移動量を指定し、オブジェクトを動かす
		_characterController.Move(moveDelta);

		if (_inputMove != Vector2.zero)
		{
			// 移動入力がある場合は、振り向き動作も行う

			// 操作入力からy軸周りの目標角度[deg]を計算
			float targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
				* Mathf.Rad2Deg + 90;

			// イージングしながら次の回転角度[deg]を計算
			float angleY = Mathf.SmoothDampAngle(
				_transform.eulerAngles.y,
				targetAngleY,
				ref _turnVelocity,
				0.1f
			);

			// オブジェクトの回転を更新
			_transform.rotation = Quaternion.Euler(0, angleY, 0);
		}
	}
}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] float rotationSpeed = 100f;
	[SerializeField] float verticalSpeed = 5f;

	private Rigidbody rb;
	private PlayerControls controls;
	private Vector2 moveInput;
	private bool ascendPressed;
	private bool descendPressed;

	void Awake()
	{
		controls = new PlayerControls();

		controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

		controls.Player.Ascend.performed += ctx => ascendPressed = true;
		controls.Player.Ascend.canceled += ctx => ascendPressed = false;

		controls.Player.Descend.performed += ctx => descendPressed = true;
		controls.Player.Descend.canceled += ctx => descendPressed = false;
	}

	void OnEnable() => controls.Enable();
	void OnDisable() => controls.Disable();

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	void Update()
	{
		HandleMovement();
	}

	void HandleMovement()
	{
		// 水平移動（前進/後退、旋回）
		float forwardInput = moveInput.y;
		float turnInput = moveInput.x;

		Vector3 forwardVelocity = transform.forward * forwardInput * moveSpeed;
		float turn = turnInput * rotationSpeed * Time.deltaTime;
		transform.Rotate(0, turn, 0);

		// 上下移動（浮上・潜行）
		float vertical = 0f;
		if (ascendPressed) vertical += 1f;
		if (descendPressed) vertical -= 1f;

		Vector3 verticalVelocity = Vector3.up * vertical * verticalSpeed;

		// 合成して適用
		rb.velocity = forwardVelocity + verticalVelocity;
	}
}
*/