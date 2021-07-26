using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class LevelManager : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject[] _disableOnFinish;
		[SerializeField] private int _baseCoinsPerVictory = 10;
		[SerializeField] private float _skinBonusPerVictory = 0.1f;
		[SerializeField, HideInInspector] private MapGeneration _map;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private CameraResetter _cameraResetter;
		[SerializeField, HideInInspector] private LetterWriteSystem _letterSystem;
		[SerializeField, HideInInspector] private DailyChallenge _dailyChallenge;
		[SerializeField, HideInInspector] private Income _income;
		[SerializeField, HideInInspector] private Shop _shop;
		public static event System.Action<bool> OnLevelEnd;
		private bool _isDailyLevel = false;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_shop = FindObjectOfType<Shop>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_coins = FindObjectOfType<CoinManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_cameraResetter = FindObjectOfType<CameraResetter>(true);
				_map = FindObjectOfType<MapGeneration>(true);
				_letterSystem = FindObjectOfType<LetterWriteSystem>(true);
				_dailyChallenge = FindObjectOfType<DailyChallenge>(true);
				_income = FindObjectOfType<Income>(true);
			}
		}
		
		public void StartDailyLevel()
		{
			_isDailyLevel = true;
			StartLevel();
			_map.ResetToDaily();
		}
		
		public void StartLevel()
		{
			_letterSystem.Reset();
		}
		
		public void FinishLevel(bool victory, int manikinsCollected, float coinsMultiplier = 1f)
		{
			if(victory)
			{
				_stats.CurrentLevel++;
				_coins.AddEarnedCoins((int)((float)_baseCoinsPerVictory * coinsMultiplier) + _income.GetBonusCoins());
				_stats.SkinBonusProgress += _skinBonusPerVictory;
				if(_stats.SkinBonusProgress > 1f)
				{
					_stats.SkinBonusProgress = 0f;
					_shop.UnlockRandomSkin();
				}
				
				if(_isDailyLevel)
				{
					float deilyProgress = (float)(manikinsCollected - 1) / (float)(_map.ManikinsAmount - 1) * 100f;
					_dailyChallenge.TryUpdateDailyProgress(deilyProgress);
					//print("deilyProgress: " + (int)deilyProgress + "%");
				}
			}
			LevelEnd();
			OnLevelEnd?.Invoke(victory);
		}
		
		private void LevelEnd()
		{
			Time.timeScale = 0f;
			ResetLevel();
		}
		
		private void ResetLevel()
		{
			_isDailyLevel = false;
			foreach(var p in _disableOnFinish)
				p.SetActive(false);
			_map.Reset();
			_player.Reset();
			_stats.Reset();
			_cameraResetter.Reset();
		}
	}
}