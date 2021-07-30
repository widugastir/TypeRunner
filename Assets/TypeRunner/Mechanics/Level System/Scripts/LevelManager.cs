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
		[SerializeField, HideInInspector] private MapGeneration _map;
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
			_gameCanvas.SetActive(true);
			_letterSystem.Reset();
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
				_coins.AddEarnedCoins((int)((float)_baseCoinsPerVictory * coinsMultiplier) + _income.GetBonusCoins());
				
				if(_isDailyLevel)
				{
					float deilyProgress = (float)(manikinsCollected - 1) / (float)(_map.ManikinsAmount - 1) * 100f;
					_dailyChallenge.TryUpdateDailyProgress(deilyProgress);
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
		}
		
		public void FinishLevel()
		{
			OnLevelEnd?.Invoke(_isVictory);
			ResetLevel();
		}
		
		private void ResetLevel()
		{
			_isVictory = false;
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