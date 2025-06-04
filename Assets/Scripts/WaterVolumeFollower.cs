using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolumeFollower : MonoBehaviour
{
	[SerializeField] private Transform player;

    void Update()
    {
        Vector3 pos = player.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }
}
