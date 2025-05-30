using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip normalAudio;
	[SerializeField] private AudioClip underwaterAudio;
	[SerializeField] private float fadeDuration = 1.5f;

	private AudioClip currentClip;
	private Coroutine fadeCoroutine;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void PlayAudio(AudioClip clip)
	{
		if (currentClip == clip || clip == null) return;

		currentClip = clip;

		if (fadeCoroutine != null)
			StopCoroutine(fadeCoroutine);

		fadeCoroutine = StartCoroutine(FadeToNewClip(clip));
	}

	public void PlayNormalAudio() => PlayAudio(normalAudio);
	public void PlayUnderwaterAudio() => PlayAudio(underwaterAudio);

	private IEnumerator FadeToNewClip(AudioClip newClip)
	{
		float startVolume = audioSource.volume;

		// フェードアウト
		for (float t = 0; t < fadeDuration; t += Time.deltaTime)
		{
			audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
			yield return null;
		}

		audioSource.Stop();
		audioSource.clip = newClip;
		audioSource.Play();

		// フェードイン
		for (float t = 0; t < fadeDuration; t += Time.deltaTime)
		{
			audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
			yield return null;
		}

		audioSource.volume = startVolume;
	}
}
