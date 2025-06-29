using UnityEngine;
using Cinemachine;

public class LockOnCinemachine : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float lockOnRange = 15f;
    [SerializeField] private string[] ballTags = { "CorrectBall", "Ball" };

    private Transform lockTarget;

    void Start()
    {
        virtualCamera.m_Lens.FieldOfView = 90f;

        // 初期状態：プレイヤーのみ
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[1] {
            new CinemachineTargetGroup.Target { target = player, weight = 1f, radius = 1f }
        };
    }

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
            float dist = Vector3.Distance(player.position, lockTarget.position);
            if (dist > lockOnRange || !lockTarget.gameObject.activeInHierarchy)
                Unlock();
        }
    }

    void TryLockOn()
    {
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (string tag in ballTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject enemy in enemies)
            {
                float dist = Vector3.Distance(player.position, enemy.transform.position);
                if (dist < lockOnRange && dist < closestDist)
                {
                    closest = enemy.transform;
                    closestDist = dist;
                }
            }
        }

        if (closest != null)
        {
            lockTarget = closest;
            targetGroup.m_Targets = new CinemachineTargetGroup.Target[2] {
                new CinemachineTargetGroup.Target { target = player, weight = 1f, radius = 1f },
                new CinemachineTargetGroup.Target { target = lockTarget, weight = 1f, radius = 1f }
            };
        }
    }

    void Unlock()
    {
        lockTarget = null;

        targetGroup.m_Targets = new CinemachineTargetGroup.Target[1] {
            new CinemachineTargetGroup.Target { target = player, weight = 1f, radius = 1f }
        };
    }
}
