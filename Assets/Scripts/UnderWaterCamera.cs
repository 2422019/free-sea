using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UnderWaterCamera : MonoBehaviour
{
	private float waterHeight = 0f;
	private Color underwaterFogColor = new Color(0f, 0.4f, 0.6f, 1f);
	private float underwaterFogDensity = 0.05f;

	private Color defaultFogColor;
	private float defaultFogDensity;
	private bool defaultFogEnabled;

	private void Start()
	{
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
		defaultFogEnabled = RenderSettings.fog;
	}

	private void Update()
	{
		if (transform.position.y < waterHeight)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = underwaterFogColor;
			RenderSettings.fogDensity = underwaterFogDensity;
		}
		else
		{
			RenderSettings.fog = defaultFogEnabled;
			RenderSettings.fogColor = defaultFogColor;
			RenderSettings.fogDensity = defaultFogDensity;
		}
	}
}
