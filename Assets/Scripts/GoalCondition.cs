using UnityEngine;

public class GoalCondition : MonoBehaviour
{
    [Header("正解のボールタグ")]
    [SerializeField] private string correctTag = "CorrectBall";

    [Header("サウンド設定")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip failClip;

    private void OnTriggerEnter(Collider other)
    {
        // Ballタグを持っていないものは無視
        if (!other.CompareTag("Ball") && !other.CompareTag("CorrectBall")) return;

        // 正解判定
        if (other.CompareTag(correctTag))
        {
            Debug.Log("正解のボール！");
            PlaySound(successClip);
        }
        else
        {
            Debug.Log("間違ったボール！");
            PlaySound(failClip);
        }

        // オブジェクトプールに返却または削除
        var poolRef = other.GetComponent<ObjectPoolReference>();
        if (poolRef != null && poolRef.Pool != null)
        {
            poolRef.Pool.Return(other.gameObject);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
