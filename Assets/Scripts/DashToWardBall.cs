using UnityEngine;

public class DashTowardBall : MonoBehaviour
{
    [Header("�ݒ�")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float lockOnRange = 15f;
    [SerializeField] private Transform headTransform; // �w�f�B���O�p�̈ʒu�i���j

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    private CharacterController controller;
    private Vector3 dashDirection;

    private Transform currentTarget;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �N�[���^�C���X�V
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer < dashDuration)
            {
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            }
            else
            {
                isDashing = false;
                cooldownTimer = dashCooldown;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
            {
                currentTarget = FindNearestBall();
                if (currentTarget != null)
                {
                    Vector3 toBall = currentTarget.position - headTransform.position;

                    // �ːi�����𓪂���{�[���֌�����
                    dashDirection = toBall.normalized;

                    isDashing = true;
                    dashTimer = 0f;

                    // �������{�[���Ɍ�����
                    Vector3 flatDir = new Vector3(dashDirection.x, 0f, dashDirection.z);
                    if (flatDir != Vector3.zero)
                        transform.rotation = Quaternion.LookRotation(flatDir);
                }
            }
        }
    }

    private Transform FindNearestBall()
    {
        string[] tags = { "CorrectBall", "Ball" };
        float closestDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (string tag in tags)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject ball in balls)
            {
                float dist = Vector3.Distance(transform.position, ball.transform.position);
                if (dist < closestDist && dist <= lockOnRange)
                {
                    closestDist = dist;
                    nearest = ball.transform;
                }
            }
        }

        return nearest;
    }
}
