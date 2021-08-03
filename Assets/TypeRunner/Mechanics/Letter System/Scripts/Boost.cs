using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class Boost : MonoBehaviour
	{
		[SerializeField] private float _bonusDuration = 3f;
		
		[SerializeField] private ParticleSystem _speedParticles;
		[SerializeField] private Image _bonusLine;
		[SerializeField] private Image _timeLine;
		[SerializeField] private float _boostMultiplier = 1.5f;
		[SerializeField] private float _fullTimer = 10f;
		[SerializeField] private float _bonusTimer = 2f;
		[SerializeField] private MapMovement _mapMovement;
		[SerializeField] private PlayerController _playerController;
		private float _timer = 0f;
		private BoostTimerResult _result = BoostTimerResult.unsuccessful;
		public BoostTimerResult Result => _result;
		private IEnumerator _bonusCoroutine;
		private IEnumerator _speedCoroutine;
		
		// {1} - successful {2} - bonus
		private System.Action<bool, bool> _endCallback;
	    
		protected void OnDisable()
		{
			if(_bonusCoroutine != null)
				StopCoroutine(_bonusCoroutine);
		}
	    
		public void ApplyBonus()
		{
			_playerController.SetImmortal(_bonusDuration, true);
			_mapMovement._moveSpeed *= _boostMultiplier;
			if(_bonusCoroutine != null)
			{
				StopCoroutine(_bonusCoroutine);
				_bonusCoroutine = null;
			}
			_bonusCoroutine = DisableBonus();
			EnableSpeedParticles();
			StartCoroutine(_bonusCoroutine);
		}
		
		private void EnableSpeedParticles()
		{
			if(_speedCoroutine != null)
			{
				StopCoroutine(_speedCoroutine);
				_speedCoroutine = null;
			}
			_speedCoroutine = DisableSpeedParticles();
			StartCoroutine(_speedCoroutine);
			_speedParticles.Play();
		}
		
		private IEnumerator DisableSpeedParticles()
		{
			yield return new WaitForSecondsRealtime(_bonusDuration);
			_speedParticles.Stop();
			_speedCoroutine = null;
		}
		
		private IEnumerator DisableBonus()
		{
			yield return new WaitForSecondsRealtime(_bonusDuration);
			_mapMovement.ResetSpeed();
			_bonusCoroutine = null;
		}
	    
		public void EnableTimer(System.Action<bool, bool> endCallback)
		{
			_result = BoostTimerResult.unsuccessful;
			_endCallback = endCallback;
			_timer = _fullTimer;
			_timeLine.fillAmount = 1f;
			_bonusLine.fillAmount = _bonusTimer / _fullTimer;
		}
		
		private void Update()
		{
			if(_timer > 0f)
			{
				_timer -= Time.unscaledDeltaTime;
				if(_timer < 0f)
				{
					StopTimer();
					_timer = 0f;
				}
				UpdateUI();
			}
		}
		
		public void StopTimer()
		{
			if(_timer <= 0f)
			{
				_result = BoostTimerResult.unsuccessful;
			}
			else if(_timer >= (_fullTimer - _bonusTimer))
			{
				_result = BoostTimerResult.bonus;
			}
			else _result = BoostTimerResult.successful;
			
			_timer = 0f;
			_endCallback?.Invoke(false, false);
		}
		
		private void UpdateUI()
		{
			_timeLine.fillAmount = _timer / _fullTimer;
		}
		
		public enum BoostTimerResult
		{
			unsuccessful,
			successful,
			bonus
		}
	}
}