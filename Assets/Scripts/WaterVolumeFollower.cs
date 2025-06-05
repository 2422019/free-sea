using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolumeFollower : MonoBehaviour
{
	[SerializeField] private Transform player;
	[Header("óhÇÍÇÈäpìx")]
	[SerializeField] float angleRange = 10f;

	[Header("óhÇÍÇÃë¨Ç≥")]
	[SerializeField] float speed = 1f;

	private float startY;

	void Start()
	{
		startY = transform.eulerAngles.y;
	}

	void Update()
    {
        Vector3 pos = player.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
		// è∞ÇóhÇÁÇ∑
		float angle = Mathf.Sin(Time.time * speed) * angleRange;
		transform.rotation = Quaternion.Euler(0f, startY + angle, 0f);
    }
}
