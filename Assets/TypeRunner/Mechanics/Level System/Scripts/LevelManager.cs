using System.Collections.Generic;
using System.Collections;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class LevelManager : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject[] _disableOnFinish;
		[SerializeField] private GameObject _gameCanvas;
		[SerializeField] private int _baseCoinsPerVictory = 10;
		[SerializeField] private int _bonusSkinPerLevel = 5;
		[SerializeField, HideInInspector] private MapGenerationLevels _map;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private CameraResetter _cameraResetter;
		[SerializeField, HideInInspector] private LetterWriteSystem _letterSystem;
		[SerializeField, HideInInspector] private DailyChallenge _dailyChallenge;
		[SerializeField, HideInInspector] private GameEndPanel _endPanel;
		[SerializeField, HideInInspector] private Income _income;
		[SerializeField, HideInInspector] private Shop _shop;
		public static event System.Action<bool> OnLevelEnd;
		[HideInInspector] public bool _isDailyLevel = false;
		private bool _isVictory = false;
		private IEnumerator _timer;
		private float _levelTimer = 0f;

		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_endPanel = FindObjectOfType<GameEndPanel>(true);
				_shop = FindObjectOfType<Shop>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_coins = FindObjectOfType<CoinManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_cameraResetter = FindObjectOfType<CameraResetter>(true);
				_map = FindObjectOfType<MapGenerationLevels>(true);
				_letterSystem = FindObjectOfType<LetterWriteSystem>(true);
				_dailyChallenge = FindObjectOfType<DailyChallenge>(true);
				_income = FindObjectOfType<Income>(true);
			}
		}
		
		private void SendFinishLevelMetrics()
		{
			Dictionary<string, object > parameters = new Dictionary<string, object>();
			parameters.Add("level_number", _stats.CurrentLevel);
			parameters.Add("level_count", _stats.LevelPlayCount);
			parameters.Add("result", _isVictory ? "win" : "lose");
			parameters.Add("time", _levelTimer.ToString("0.00"));
			AppMetrica.Instance.ReportEvent("level_finish", parameters);
			AppMetrica.Instance.SendEventsBuffer();
		}
	
		private void SendStartLevelMetrics()
		{
			Dictionary<string, object > parameters = new Dictionary<string, object>();
			parameters.Add("level_number", _stats.CurrentLevel);
			parameters.Add("level_count", _stats.LevelPlayCount);
			AppMetrica.Instance.ReportEvent("level_start", parameters);
			AppMetrica.Instance.SendEventsBuffer();
		}
		
		public void StartDailyLevel()
		{
			_stats.LevelPlayCount++;
			_isDailyLevel = true;
			StartLevel();
			_map.ResetToDaily();
			StartCoroutine(_timer);
			SendStartLevelMetrics();
		}
		
		public void StartLevel()
		{
			_stats.LevelPlayCount++;
			_gameCanvas.SetActive(true);
			_letterSystem.Reset();
			_timer = LevelTimer();
			StartCoroutine(_timer);
			SendStartLevelMetrics();
		}
		
		public void PreFinishLevel(bool victory, int manikinsCollected, float coinsMultiplier = 1f)
		{
			
			Time.timeScale = 0f;
			
			_isVictory = victory;
			if(victory)
			{
				if(_isDailyLevel == false)
				{
					_stats.CurrentLevel++;
					_stats.SkinBonusProgress += (float)(1f / (_bonusSkinPerLevel * (_stats._bonusSkinGained + 1)));
					if(_stats.SkinBonusProgress >= 1f)
					{
						_stats.SkinBonusProgress = 1f;
						_stats._bonusSkinGained++;
						_shop.UnlockRandomSkin();
					}
				}
				
				
				if(_isDailyLevel)
				{
					float deilyProgress = (float)(manikinsCollected - 1) / (float)(_map.ManikinsAmount - 1) * 100f;
					_dailyChallenge.TryUpdateDailyProgress(deilyProgress);
				}
				else
				{
					_coins.AddEarnedCoins((int)((float)_baseCoinsPerVictory * coinsMultiplier) + _income.GetBonusCoins());
				}
				
				
				if(_isDailyLevel)
					_endPanel.Enable(victory, false);
				else
					_endPanel.Enable(victory, true);
					
			}
			else
			{
				_endPanel.Enable(victory, false);
			}
			_gameCanvas.SetActive(false);
			
			SendFinishLevelMetrics();
			if(_timer != null)
			{
				StopCoroutine(_timer);
				_timer = null;
			}
		}
		
		public void FinishLevel()
		{
			OnLevelEnd?.Invoke(_isVictory);
			ResetLevel();
		}
		
		private IEnumerator LevelTimer()
		{
			while(true)
			{
				yield return new WaitForEndOfFrame();
				_levelTimer += Time.deltaTime;
			}
		}
		
		private void ResetLevel()
		{
			_letterSystem.Reset();
			_isVictory = false;
			_isDailyLevel = false;
			foreach(var p in _disableOnFinish)
				p.SetActive(false);
			_player.Reset();
			_map.Reset();
			_stats.Reset();
			_cameraResetter.Reset();
		}
	}
}