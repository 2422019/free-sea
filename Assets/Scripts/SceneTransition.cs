using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	[SerializeField] private Image fadeImage;
	[SerializeField] private float fadeDuration = 1.0f;
	[SerializeField] private string gameSceneName = "GameScene";
	[SerializeField] private AudioSource bgmSource;

	private bool isFading = false;

	void Update()
	{
		if (!isFading && Input.GetMouseButtonDown(0))
		{
			StartCoroutine(FadeAndLoadScene());
		}
	}

	private IEnumerator FadeAndLoadScene()
	{
		isFading = true;

		float timer = 0f;
		Color color = fadeImage.color;
		float startVolume = bgmSource != null ? bgmSource.volume : 1f;

		while (timer < fadeDuration)
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp01(timer / fadeDuration);

			// �t�F�[�h�摜
			fadeImage.color = new Color(color.r, color.g, color.b, t);

			// BGM ���ʂ����X�ɉ�����
			if (bgmSource != null)
			{
				bgmSource.volume = Mathf.Lerp(startVolume, 0f, t);
			}

			yield return null;
		}

		// �t�F�[�h��Ɋ��S��~
		if (bgmSource != null)
		{
			bgmSource.Stop();
		}

		SceneManager.LoadScene(gameSceneName);
	}
}
