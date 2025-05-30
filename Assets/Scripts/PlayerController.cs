using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("�ړ����x"), SerializeField] private float _moveSpeed = 5f;
	[Header("�㏸�E���~���x"), SerializeField] private float _verticalSpeed = 3f;
	[Header("��]�̃X���[�Y��"), SerializeField] private float _turnSmoothTime = 0.1f;

	private Vector2 _inputHorizontal; // ���X�e�B�b�N�p�iXZ�ړ��j
	private float _inputVertical;     // �㉺�ړ��p�i��FA/Z, �g���K�[�j

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

		// ���������炩�ɕύX�iXZ�����̂݁j
		if (inputDir.magnitude > 0.1f)
		{
			float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothTime);
			_transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}

		// �N�W�������̒��ɂ��邩�ǂ�����Audio��؂�ւ���
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
	[Header("�ړ��̑���"), SerializeField]
	private float _speed = 3;

	[Header("�W�����v����u�Ԃ̑���"), SerializeField]
	private float _jumpSpeed = 7;

	[Header("�d�͉����x"), SerializeField]
	private float _gravity = 15;

	[Header("�������̑��������iInfinity�Ŗ������j"), SerializeField]
	private float _fallSpeed = 10;

	[Header("�����̏���"), SerializeField]
	private float _initFallSpeed = 2;

	private Transform _transform;
	private CharacterController _characterController;

	private Vector2 _inputMove;
	private float _verticalVelocity;
	private float _turnVelocity;
	private bool _isGroundedPrev;

	public void OnMove(InputAction.CallbackContext context)
	{
		// ���͒l��ێ����Ă���
		_inputMove = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		// �{�^���������ꂽ�u�Ԃ����n���Ă��鎞��������
		if (!context.performed || !_characterController.isGrounded) return;

		// ����������ɑ��x��^����
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
			// ���n����u�Ԃɗ����̏������w�肵�Ă���
			_verticalVelocity = -_initFallSpeed;
		}
		else if (!isGrounded)
		{
			// �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
			_verticalVelocity -= _gravity * Time.deltaTime;

			// �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
			if (_verticalVelocity < -_fallSpeed)
				_verticalVelocity = -_fallSpeed;
		}

		_isGroundedPrev = isGrounded;

		// ������͂Ɖ����������x����A���ݑ��x���v�Z
		Vector3 moveVelocity = new Vector3(
			_inputMove.x * _speed,
			_verticalVelocity,
			_inputMove.y * _speed
		);
		// ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
		Vector3 moveDelta = moveVelocity * Time.deltaTime;

		// CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
		_characterController.Move(moveDelta);

		if (_inputMove != Vector2.zero)
		{
			// �ړ����͂�����ꍇ�́A�U�����������s��

			// ������͂���y������̖ڕW�p�x[deg]���v�Z
			float targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
				* Mathf.Rad2Deg + 90;

			// �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
			float angleY = Mathf.SmoothDampAngle(
				_transform.eulerAngles.y,
				targetAngleY,
				ref _turnVelocity,
				0.1f
			);

			// �I�u�W�F�N�g�̉�]���X�V
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
		// �����ړ��i�O�i/��ށA����j
		float forwardInput = moveInput.y;
		float turnInput = moveInput.x;

		Vector3 forwardVelocity = transform.forward * forwardInput * moveSpeed;
		float turn = turnInput * rotationSpeed * Time.deltaTime;
		transform.Rotate(0, turn, 0);

		// �㉺�ړ��i����E���s�j
		float vertical = 0f;
		if (ascendPressed) vertical += 1f;
		if (descendPressed) vertical -= 1f;

		Vector3 verticalVelocity = Vector3.up * vertical * verticalSpeed;

		// �������ēK�p
		rb.velocity = forwardVelocity + verticalVelocity;
	}
}
*/