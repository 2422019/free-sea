using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolumeFollower : MonoBehaviour
{
	[SerializeField] private Transform player;
	[Header("�h���p�x")]
	[SerializeField] float angleRange = 10f;

	[Header("�h��̑���")]
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
		// ����h�炷
		float angle = Mathf.Sin(Time.time * speed) * angleRange;
		transform.rotation = Quaternion.Euler(0f, startY + angle, 0f);
    }
}
