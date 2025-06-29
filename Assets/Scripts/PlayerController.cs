using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("�ړ����x"), SerializeField] private float _moveSpeed = 5f;
	[Header("�㏸�E���~���x"), SerializeField] private float _verticalSpeed = 3f;
	[Header("��]�̃X���[�Y��"), SerializeField] private float _turnSmoothTime = 0.1f;

	[Header("����p�x�̐���"), SerializeField] private float _maxPitchAngle = 45f;
	[Header("�㉺��]�̃X���[�Y��"), SerializeField] private float _pitchSmoothTime = 0.2f;

	[Header("�ړ��ł��鍂���̐���")]
	[SerializeField] private float _minY = -20f;
	[SerializeField] private float _maxY = 5f;

	[Header("�C�ʁE���̃I�u�W�F�N�g")]
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

		// �J�[�\�����b�N
		Cursor.lockState = CursorLockMode.Locked;

		// �J�[�\����\��
		Cursor.visible = false;
	}

	private void Update()
	{
		Transform cam = Camera.main.transform;

		// �J�����
		Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 camRight = cam.right;

		Vector3 inputDir = new Vector3(_inputHorizontal.x, 0f, _inputHorizontal.y).normalized;
		Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;
		Vector3 verticalMove = Vector3.up * _inputVertical;

		Vector3 move = (moveDir * _moveSpeed + verticalMove * _verticalSpeed) * Time.deltaTime;
		_controller.Move(move);

		// ��������
		Vector3 pos = _transform.position;
		pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
		_transform.position = pos;

		// ������]�iYaw�j
		if (moveDir.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothTime);
			_transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, angle, 0f);
		}

		// �㉺��]�iPitch�j - �O�i���̂ݔ��f
		float forwardAmount = Vector3.Dot(moveDir.normalized, _transform.forward);
		float targetPitch = -_inputVertical * _maxPitchAngle * Mathf.Clamp01(forwardAmount);
		_currentPitch = Mathf.SmoothDamp(_currentPitch, targetPitch, ref _pitchVelocity, _pitchSmoothTime);

		// ��]��Pitch���f
		float currentYaw = _transform.eulerAngles.y;
		_transform.rotation = Quaternion.Euler(_currentPitch, currentYaw, 0f);

		// �C�ʂ̒Ǐ]
		if (surfaceFloor.activeSelf)
		{
			Vector3 floorPos = surfaceFloor.transform.position;
			floorPos.x = _transform.position.x;
			floorPos.z = _transform.position.z;
			surfaceFloor.transform.position = floorPos;
		}

		// ����/���㔻��ƌ��ʐ؂�ւ�
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
