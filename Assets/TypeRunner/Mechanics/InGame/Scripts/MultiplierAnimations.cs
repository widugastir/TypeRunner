using SoundSteppe.RefSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace TypeRunner
{
	public class MultiplierAnimations : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private Animator _animator;
		[SerializeField] private TMP_Text _multiplierText;
		[SerializeField] private float _numericAnimDuration = 1f;
		[SerializeField] private float _numericAnimStep = 0.1f;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private PlayerController _player;
		
		private Action _onMultiplyAnim;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_player = FindObjectOfType<PlayerController>(true);
			}
		}
		
		public void PlayNumericAnim(Action onMultiplyAnim)
		{
			_onMultiplyAnim = onMultiplyAnim;
			float startMultiplier = 1f;
			_multiplierText.text = "X " + startMultiplier.ToString("0.00");
			_animator.SetTrigger("ZoomIN");
		}
		
		private void ZoomInEnd()
		{
			StartCoroutine(NumericAnim());
		}
		
		private IEnumerator NumericAnim()
		{
			float startMultiplier = 1f;
			float currentMultiplier = startMultiplier;
			float targetMultiplier = _stats.SuccessfulMultiplier;
			_multiplierText.text = "X " + startMultiplier.ToString("0.00");
			float timePerStep = _numericAnimDuration / ((targetMultiplier - startMultiplier) / _numericAnimStep);
			//print(timePerStep);
			_player.MultiplyStikmans(_stats.SuccessfulMultiplier, _numericAnimDuration);
			while(currentMultiplier < targetMultiplier)
			{
				yield return new WaitForSecondsRealtime(timePerStep);
				currentMultiplier += _numericAnimStep;
				_multiplierText.text = "X " + currentMultiplier.ToString("0.00");
			}
			_multiplierText.text = "X " + targetMultiplier.ToString("0.00");
			_animator.SetTrigger("ZoomOUT");
			_onMultiplyAnim?.Invoke();
		}
	}
}