using UnityEngine;
using Cinemachine;

public class LockOnCinemachine : MonoBehaviour
{
	[SerializeField] private Transform player;
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private float lockOnRange = 15f;
	[SerializeField] private string enemyTag = "Ball";

	private Transform lockTarget;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (lockTarget == null)
				TryLockOn();
			else
				Unlock();
		}

		if (lockTarget != null)
		{
			// ロック対象が消えたら解除
			float dist = Vector3.Distance(player.position, lockTarget.position);
			if (dist > lockOnRange || !lockTarget.gameObject.activeInHierarchy)
				Unlock();
		}
	}

	void TryLockOn()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float closestDist = Mathf.Infinity;
		Transform closest = null;

		foreach (GameObject enemy in enemies)
		{
			float dist = Vector3.Distance(player.position, enemy.transform.position);
			if (dist < lockOnRange && dist < closestDist)
			{
				closest = enemy.transform;
				closestDist = dist;
			}
		}

		if (closest != null)
		{
			lockTarget = closest;
			virtualCamera.LookAt = lockTarget; // ロックオン
		}
	}

	void Unlock()
	{
		lockTarget = null;
		virtualCamera.LookAt = player; // ロック解除
	}
}
