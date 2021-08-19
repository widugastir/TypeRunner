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
		[SerializeField, HideInInspector] private MapMovement _movement;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private PlayerController _player;
		
		private Action _onMultiplyAnim;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_movement = FindObjectOfType<MapMovement>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_player = FindObjectOfType<PlayerController>(true);
			}
		}
		
		public void PlayNumericAnim(Action onMultiplyAnim)
		{
			_onMultiplyAnim = onMultiplyAnim;
			float startMultiplier = 1f;
			_multiplierText.text = "X " + startMultiplier.ToString("0.00");
			_movement.CanMove = false;
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
			if(targetMultiplier != startMultiplier)
			{
				_multiplierText.text = "X " + startMultiplier.ToString("0.00");
				float different = targetMultiplier - startMultiplier;
				//float timePerStep = _numericAnimDuration * _numericAnimStep / different;
				
				float stepsAmount = 15f;
				float _animDelay = _numericAnimDuration / stepsAmount;
				float _multiplStep = different / stepsAmount;
				
				//print(timePerStep + "   " );
				
				
				_player.MultiplyStikmans(_stats.SuccessfulMultiplier, _numericAnimDuration, MultiplyEnd);
				while(currentMultiplier < targetMultiplier)
				{
					yield return new WaitForSecondsRealtime(_animDelay);
					currentMultiplier += _multiplStep;
					_multiplierText.text = "X " + currentMultiplier.ToString("0.00");
				}
			}
			else
			{
				yield return new WaitForSecondsRealtime(_numericAnimDuration);
				MultiplyEnd();
			}
			
			_multiplierText.text = "X " + targetMultiplier.ToString("0.00");
			_animator.SetTrigger("ZoomOUT");
		}
		
		private void MultiplyEnd()
		{
			_movement.CanMove = true;
			_onMultiplyAnim?.Invoke();
		}
	}
}