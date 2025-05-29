using UnityEngine;

public class WaterManager : MonoBehaviour
{
	public static WaterManager Instance { get; private set; }

	[Header("���ʂ̍����ݒ�")]
	[SerializeField] float waterHeight = 0f;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
	}

	// �w�肵�����[���h���W������������itrue = �����j
	public bool IsUnderwater(Vector3 worldPosition)
	{
		return worldPosition.y < waterHeight;
	}

	// �g�����X�t�H�[���������ɂ��邩�ǂ����itrue = �����j
	public bool IsUnderwater(Transform transform)
	{
		return IsUnderwater(transform.position);
	}
}
