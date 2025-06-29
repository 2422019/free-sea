using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    [Header("UI・フェード設定")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("フェードアウトするテキスト")] 
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

        // 初期状態のテキスト色を取得
        Color textColor = textToFade != null ? textToFade.color : Color.white;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // フェード画像の透明度を上げる
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, t);

            // BGM 音量を下げる
            if (bgmSource != null)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t);
            }

            // テキストの透明度を下げる
            if (textToFade != null)
            {
                Color c = textColor;
                c.a = 1f - t;
                textToFade.color = c;
            }

            yield return null;
        }

        // フェード完了後にBGM停止
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        // シーン遷移
        SceneManager.LoadScene(gameSceneName);
    }
}
