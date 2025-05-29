using UnityEngine;

public class WaterManager : MonoBehaviour
{
	public static WaterManager Instance { get; private set; }

	[Header("水面の高さ設定")]
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

	// 指定したワールド座標が水中か判定（true = 水中）
	public bool IsUnderwater(Vector3 worldPosition)
	{
		return worldPosition.y < waterHeight;
	}

	// トランスフォームが水中にあるかどうか（true = 水中）
	public bool IsUnderwater(Transform transform)
	{
		return IsUnderwater(transform.position);
	}
}
