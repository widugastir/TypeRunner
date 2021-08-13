using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class Boost : MonoBehaviour
	{
		[SerializeField] private float _bonusDuration = 3f;
		
		[SerializeField] private ParticleSystem _speedParticles;
		[SerializeField] private Slider _timerSlider;
		[SerializeField] private RectTransform _mainLine;
		[SerializeField] private RectTransform _bonusLine;
		[SerializeField] private float _boostMultiplier = 1.5f;
		[SerializeField] private float _fullTimer = 10f;
		[SerializeField] private float _bonusTimer = 2f;
		[SerializeField] private LetterWriteSystem _letterSystem;
		[SerializeField] private MapMovement _mapMovement;
		[SerializeField] private PlayerController _playerController;
		private float _timer = 0f;
		private BoostTimerResult _result = BoostTimerResult.unsuccessful;
		public BoostTimerResult Result => _result;
		private IEnumerator _bonusCoroutine;
		private IEnumerator _speedCoroutine;
		private float _baseFullTimer;
		private float _baseBonusTimer;
		
		// {1} - successful {2} - bonus
		private System.Action<bool, bool, ObstacleZone> _endCallback;
	    
		private void Start()
		{
			_baseFullTimer = _fullTimer;
			_baseBonusTimer = _bonusTimer;
		}
		
		public void ResetTimer()
		{
			StopAllCoroutines();
			_timer = 0f;
			_fullTimer = _baseFullTimer;
			_bonusTimer = _baseBonusTimer;
			_oneUseTrigger = true;
			_letterSystem.DisableOutline();
		}
	    
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
			_letterSystem.DisableOutline();
			_mapMovement.ResetSpeed();
			_bonusCoroutine = null;
		}
		
		private void UpdateTimerUI()
		{
			_timerSlider.value = 1f;
			float ratio = 1f - _bonusTimer / _fullTimer;
			Vector2 newSize = _mainLine.offsetMax;
			newSize.x = _bonusLine.sizeDelta.x * ratio;
			_mainLine.offsetMax = newSize;
			
			if(_bonusTimer > 0f)
				_letterSystem.EnableOutline();
		}
		
		public void EnableTimer(FlyZone zone, System.Action<bool, bool, ObstacleZone> endCallback)
		{
			ResetTimer();
			if(zone._fullTimer > 0f)
			{
				_fullTimer = zone._fullTimer;
			}
			if(zone._bonusTimer > 0f)
			{
				_bonusTimer = zone._bonusTimer;
			}
			_result = BoostTimerResult.unsuccessful;
			_endCallback = endCallback;
			_timer = _fullTimer;
			UpdateTimerUI();
		}
		
		public void EnableTimer(ObstacleZone zone, System.Action<bool, bool, ObstacleZone> endCallback)
		{
			ResetTimer();
			if(zone._fullTimer > 0f)
			{
				_fullTimer = zone._fullTimer;
			}
			if(zone._bonusTimer > 0f)
			{
				_bonusTimer = zone._bonusTimer;
			}
			_result = BoostTimerResult.unsuccessful;
			_endCallback = endCallback;
			_timer = _fullTimer;
			UpdateTimerUI();
		}
	    
		public void EnableTimer(System.Action<bool, bool, ObstacleZone> endCallback)
		{
			ResetTimer();
			_result = BoostTimerResult.unsuccessful;
			_endCallback = endCallback;
			_timer = _fullTimer;
			UpdateTimerUI();
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
			_letterSystem.DisableOutline();
			_endCallback?.Invoke(false, false, null);
		}
		
		private void UpdateUI()
		{
			_timerSlider.value = _timer / _fullTimer;
			if(_timer < (_fullTimer - _bonusTimer))
			{
				if(OneUseTrigger)
				{
					_letterSystem.DisableOutline();
				}
			}
		}
		
		#region ONE USE ONLY TRIGGER
		private bool _oneUseTrigger = true;
		public bool OneUseTrigger
		{
			get
			{
				if(_oneUseTrigger == true)
				{
					_oneUseTrigger = false;
					return true;
				}
				return false;
			}
		}
		#endregion
		
		public enum BoostTimerResult
		{
			unsuccessful,
			successful,
			bonus
		}
	}
}