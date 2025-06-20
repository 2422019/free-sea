using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCondition : MonoBehaviour
{
	[SerializeField] private ParticleSystem goalEffect;

	void Start()
	{
		//goalEffect.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Ball"))
		{
			if (goalEffect != null)
			{
				goalEffect.transform.position = other.transform.position;
				//goalEffect.Clear;
				goalEffect.Play();
				Debug.Log("éÜêÅê·");
			}
			Debug.Log("ÉSÅ[ÉãÇµÇ‹ÇµÇΩ");
		}
	}
}
