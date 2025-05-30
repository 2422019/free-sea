using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterEffectController : MonoBehaviour
{
	[SerializeField] private Volume underwaterVolume;
	[SerializeField] private Transform checkTarget; // プレイヤーやカメラ位置
	[SerializeField] private float transitionSpeed = 2f;

	private void Update()
	{
		bool isUnderwater = WaterManager.Instance.IsUnderwater(checkTarget.position);
		float targetWeight = isUnderwater ? 1f : 0f;

		// スムーズにエフェクトを切り替え
		underwaterVolume.weight = Mathf.Lerp(underwaterVolume.weight, targetWeight, Time.deltaTime * transitionSpeed);
	}
}
