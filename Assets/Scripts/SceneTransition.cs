using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    [Header("UI�E�t�F�[�h�ݒ�")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("�t�F�[�h�A�E�g����e�L�X�g")] 
    [SerializeField] private TextMeshProUGUI textToFade;
    //[SerializeField] private Text textToFade;
    
   

    private bool isFading = false;

    void Update()
    {
        if (!isFading && Input.anyKeyDown)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        float timer = 0f;
        Color fadeColor = fadeImage.color;
        float startVolume = bgmSource != null ? bgmSource.volume : 1f;

        // ������Ԃ̃e�L�X�g�F���擾
        Color textColor = textToFade != null ? textToFade.color : Color.white;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // �t�F�[�h�摜�̓����x���グ��
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, t);

            // BGM ���ʂ�������
            if (bgmSource != null)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t);
            }

            // �e�L�X�g�̓����x��������
            if (textToFade != null)
            {
                Color c = textColor;
                c.a = 1f - t;
                textToFade.color = c;
            }

            yield return null;
        }

        // �t�F�[�h�������BGM��~
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        // �V�[���J��
        SceneManager.LoadScene(gameSceneName);
    }
}
