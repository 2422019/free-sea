using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterFlashing: MonoBehaviour
{
	[SerializeField] TextMeshProUGUI Text;

	void Start()
	{
		Text.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
		StartCoroutine("Flashing");
	}

	IEnumerator Flashing()
	{
		while (true)
		{
			for (int i = 0; i < 20; i++)
			{
				Text.color = Text.color - new Color32(0, 0, 0, 10);
				yield return new WaitForSeconds(0.01f);
			}

			for (int j = 0; j < 20; j++)
			{
				Text.color = Text.color + new Color32(0, 0, 0, 10);
				yield return new WaitForSeconds(0.01f);
			}
		}
	}
}