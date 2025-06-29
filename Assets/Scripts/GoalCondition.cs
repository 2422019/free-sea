using UnityEngine;

public class GoalCondition : MonoBehaviour
{
    [Header("�����̃{�[���^�O")]
    [SerializeField] private string correctTag = "CorrectBall";

    [Header("�T�E���h�ݒ�")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip failClip;

    private void OnTriggerEnter(Collider other)
    {
        // Ball�^�O�������Ă��Ȃ����͖̂���
        if (!other.CompareTag("Ball") && !other.CompareTag("CorrectBall")) return;

        // ���𔻒�
        if (other.CompareTag(correctTag))
        {
            Debug.Log("�����̃{�[���I");
            PlaySound(successClip);
        }
        else
        {
            Debug.Log("�Ԉ�����{�[���I");
            PlaySound(failClip);
        }

        // �I�u�W�F�N�g�v�[���ɕԋp�܂��͍폜
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
