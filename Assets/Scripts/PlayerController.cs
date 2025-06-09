using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("移動速度"), SerializeField] private float _moveSpeed = 5f;
	[Header("上昇・下降速度"), SerializeField] private float _verticalSpeed = 3f;
	[Header("回転のスムーズさ"), SerializeField] private float _turnSmoothTime = 0.1f;

	[Header("潜る角度の制限"), SerializeField] private float _maxPitchAngle = 45f;
	[Header("上下回転のスムーズさ"), SerializeField] private float _pitchSmoothTime = 0.2f;

	[Header("移動できる高さの制限"),]
	[SerializeField] private float _minY = -20f;
	[SerializeField] private float _maxY = 5f;

	[Header("海面・霧のオブジェクト")]
	[SerializeField] private GameObject surfaceFloor;
	[SerializeField] private GameObject surfaceFog;

	[SerializeField] private FogController fogController;

	private Vector2 _inputHorizontal; // 左スティック用（XZ移動）
	private float _inputVertical;     // 上下移動用（例：A/Z, トリガー）

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

		// 向きを滑らかに変更（XZ方向のみ）
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

		// クジラが水の中にいるかどうかで海の表面を表示しAudioを切り替える
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
/*	Wkeyで移動ADSでカメラの回転
 using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    }

    private void Update()
    {
        Vector3 inputDir = new Vector3(_inputHorizontal.x, 0f, _inputHorizontal.y).normalized;

        Vector3 move = Vector3.zero;

        if (inputDir.magnitude >= 0.1f)
        {
            // カメラの方向を基準に移動方向を決定
            Transform cam = Camera.main.transform;
            Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = cam.right;
            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
            moveDir.Normalize();

            // 上下移動も含めて移動量を計算
            Vector3 verticalMove = Vector3.up * _inputVertical;
            move = (moveDir * _moveSpeed + verticalMove * _verticalSpeed) * Time.deltaTime;
            _controller.Move(move);

            // 回転（水平面の向き）
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothTime);
            _transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            // 入力がない時は上下移動のみ
            Vector3 verticalMove = Vector3.up * _inputVertical;
            move = verticalMove * _verticalSpeed * Time.deltaTime;
            _controller.Move(move);
        }

        // 高さ制限
        Vector3 pos = _transform.position;
        pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
        _transform.position = pos;

        // 上下の回転（Pitch）
        float targetPitch = -_inputVertical * _maxPitchAngle;
        _currentPitch = Mathf.SmoothDamp(_currentPitch, targetPitch, ref _pitchVelocity, _pitchSmoothTime);
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

        // 水中/水上判定
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

*/
