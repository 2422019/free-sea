using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("�ړ����x"), SerializeField] private float _moveSpeed = 5f;
	[Header("�㏸�E���~���x"), SerializeField] private float _verticalSpeed = 3f;
	[Header("��]�̃X���[�Y��"), SerializeField] private float _turnSmoothTime = 0.1f;

	[Header("����p�x�̐���"), SerializeField] private float _maxPitchAngle = 45f;
	[Header("�㉺��]�̃X���[�Y��"), SerializeField] private float _pitchSmoothTime = 0.2f;

	[Header("�ړ��ł��鍂���̐���"),]
	[SerializeField] private float _minY = -20f;
	[SerializeField] private float _maxY = 5f;

	[Header("�C�ʁE���̃I�u�W�F�N�g")]
	[SerializeField] private GameObject surfaceFloor;
	[SerializeField] private GameObject surfaceFog;

	[SerializeField] private FogController fogController;

	private Vector2 _inputHorizontal; // ���X�e�B�b�N�p�iXZ�ړ��j
	private float _inputVertical;     // �㉺�ړ��p�i��FA/Z, �g���K�[�j

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
	}

	private void Update()
	{
		Vector3 inputDir = new Vector3(_inputHorizontal.x, 0f, _inputHorizontal.y).normalized;
		Vector3 verticalMove = Vector3.up * _inputVertical;

		Vector3 move = (inputDir * _moveSpeed + verticalMove * _verticalSpeed) * Time.deltaTime;
		_controller.Move(move);

		Vector3 pos = _transform.position;
		if(pos.y < _minY) pos.y = _minY;
		if(pos.y > _maxY) pos.y = _maxY;
		_transform.position = pos;

		// ���������炩�ɕύX�iXZ�����̂݁j
		if (inputDir.magnitude > 0.1f)
		{
			float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothTime);
			_transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}

		float targetPitch = -_inputVertical * _maxPitchAngle;
		_currentPitch = Mathf.SmoothDamp(_currentPitch, targetPitch, ref _pitchVelocity, _pitchSmoothTime);

		float currentYaw = _transform.eulerAngles.y;
		_transform.rotation = Quaternion.Euler(_currentPitch, currentYaw, 0f);

		bool nowUnderwater = WaterManager.Instance.IsUnderwater(transform.position);

		if(surfaceFloor.activeSelf)
		{
			Vector3 floorPos = surfaceFloor.transform.position;
			floorPos.x = _transform.position.x;
			floorPos.z = _transform.position.z;
			surfaceFloor.transform.position = floorPos;
		}

		// �N�W�������̒��ɂ��邩�ǂ����ŊC�̕\�ʂ�\����Audio��؂�ւ���
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