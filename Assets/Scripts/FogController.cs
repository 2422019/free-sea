using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour
{
	[SerializeField] private ParticleSystem fogParticle;
	[SerializeField] private float fadeDuration = 1f;

	private float currentAlpha = 1f;
	private Coroutine fadeCoroutine;

	private void Awake()
	{
		if (fogParticle == null)
		{
			Debug.LogWarning("Fog Particle is not assigned.");
		}
	}

	public void FadeIn()
	{
		if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
		fadeCoroutine = StartCoroutine(FadeTo(1f));
	}

	public void FadeOut()
	{
		if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
		fadeCoroutine = StartCoroutine(FadeTo(0f));
	}

	private IEnumerator FadeTo(float targetAlpha)
	{
		float startAlpha = currentAlpha;
		float elapsed = 0f;

		ParticleSystem.MainModule main = fogParticle.main;
		Color baseColor = main.startColor.color;

		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / fadeDuration);
			currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

			Color newColor = new Color(baseColor.r, baseColor.g, baseColor.b, currentAlpha);
			main.startColor = newColor;  // © Ä‘ã“ü‚ª•K—v

			yield return null;
		}

		currentAlpha = targetAlpha;
		Color finalColor = new Color(baseColor.r, baseColor.g, baseColor.b, currentAlpha);
		main.startColor = finalColor;
	}
}
