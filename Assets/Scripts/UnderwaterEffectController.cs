using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterEffectController : MonoBehaviour
{
	[SerializeField] private Volume underwaterVolume;
	[SerializeField] private Transform checkTarget; // �v���C���[��J�����ʒu
	[SerializeField] private float transitionSpeed = 2f;

	private void Update()
	{
		bool isUnderwater = WaterManager.Instance.IsUnderwater(checkTarget.position);
		float targetWeight = isUnderwater ? 1f : 0f;

		// �X���[�Y�ɃG�t�F�N�g��؂�ւ�
		underwaterVolume.weight = Mathf.Lerp(underwaterVolume.weight, targetWeight, Time.deltaTime * transitionSpeed);
	}
}
