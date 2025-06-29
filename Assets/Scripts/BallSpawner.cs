using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 3f;
    [Range(0, 100)]
    [SerializeField] private int correctBallChance = 70;

    [Header("プール")]
    [SerializeField] private ObjectPool correctBallPool;
    [SerializeField] private ObjectPool BallPool;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        int rand = Random.Range(0, 100);
        GameObject ball = (rand < correctBallChance)
            ? correctBallPool.Get()
            : BallPool.Get();

        if (ball != null)
        {
            ball.transform.position = spawnPoint.position;
            ball.transform.rotation = Quaternion.identity;
        }
    }
}
