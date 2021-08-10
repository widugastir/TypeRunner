using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

namespace TypeRunner
{
	public class DailyChallenge : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _minutesToPlayerUpdate = 15;
		[SerializeField] private int _attemptsPerDay = 3;
		[SerializeField] private int _updateCost = 50;
		public int UpdateCost {get {return _updateCost * (_stats._dailyUpdatesCount + 1);}}
		public int _dailyUpdateHour = 12;
		[SerializeField] private Button _updateButton;
		[SerializeField] private Button _continueButton;
		[SerializeField] private TMP_Text _updateText;
		[SerializeField] private GameObject _challengeCnavas;
		[SerializeField] private TMP_Text _attemptsText;
		[SerializeField, HideInInspector] private DailyLine[] _lines;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private GameStarter _starter;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerController _player;
	    
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_starter = FindObjectOfType<GameStarter>(true);
				_levelManager = FindObjectOfType<LevelManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_lines = GetComponentsInChildren<DailyLine>(true);
				_coins = FindObjectOfType<CoinManager>(true);
			}
		}
		
		private void OnEnable()
		{
			SaveSystem.OnEndLoad += TryUpdateAttempts;
			SaveSystem.OnBeginSave += Save;
		}
		
		private void OnDisable()
		{
			SaveSystem.OnEndLoad -= TryUpdateAttempts;
			SaveSystem.OnBeginSave -= Save;
		}
		
		public void EnableCanvas()
		{
			UpdateUI();
			_challengeCnavas.SetActive(true);
		}
		
		private void Save()
		{
			System.DateTime lastUpdate = _stats.LastDailyPlayersUpdate;
			if(lastUpdate == default)
			{
				_stats.LastDailyPlayersUpdate = DateTime.Now;
				print(_stats.LastDailyPlayersUpdate);
			}
		}
		
		private void UpdateUI()
		{
			_attemptsText.text = "Attempts: " + _stats._dailyAttempts.ToString();
			if(_stats._dailyAttempts == 0)
			{
				_continueButton.gameObject.SetActive(false);
				_updateButton.gameObject.SetActive(true);
				_updateText.text = "REFRESH: " + UpdateCost.ToString();
				if(_stats.Coins >= UpdateCost)
				{
					_updateButton.interactable = true;
				}
				else
				{
					_updateButton.interactable = false;
				}
			}
			else
			{
				_continueButton.gameObject.SetActive(true);
				_updateButton.gameObject.SetActive(false);
			}
		}
		
		public void TryBuyAttempts()
		{
			if(_coins.TrySpend(UpdateCost))
			{
				_stats._dailyUpdatesCount++;
				_stats._dailyAttempts = _attemptsPerDay;
			}
			UpdateUI();
		}
		
		private void TryUpdateHumans()
		{
			System.DateTime now = System.DateTime.Now;
			System.DateTime lastUpdate = _stats.LastDailyPlayersUpdate;
			TimeSpan span = now - lastUpdate;
			
			if(lastUpdate == default
				|| span.TotalMinutes >= _minutesToPlayerUpdate)
			{
				DailyReward[] rewards = _stats._dailyRewards;
				for(int i = 0; i < rewards.Length; i++)
				{
					rewards[i].Humans = UnityEngine.Random.Range(rewards[i].MinMaxHumans.x, rewards[i].MinMaxHumans.y);;
				}
				_stats.LastDailyPlayersUpdate = DateTime.Now;
			}
		}
		
		private void TryUpdateAttempts()
		{
			System.DateTime now = System.DateTime.Now;
			System.DateTime unlockDate = new System.DateTime(now.Year, now.Month, now.Day, _dailyUpdateHour, 0, 0);
			
			if(_stats.LastLogin < unlockDate
				&& now.Hour >= _dailyUpdateHour
				|| _stats.LastLogin == default)
			{
				_stats._dailyAttempts = _attemptsPerDay;
				_stats._dailyCategory = 0;
				_stats._dailyProcentage = 0f;
				_stats._dailyUpdatesCount = 0;
				
				DailyReward[] rewards = _stats._dailyRewards;
				for(int i = 0; i < rewards.Length; i++)
				{
					rewards[i].Humans = UnityEngine.Random.Range(rewards[i].MinMaxHumans.x, rewards[i].MinMaxHumans.y);;
				}
			}
			TryUpdateHumans();
			InitLines();
			UpdateUI();
		}
		
		private void InitLines()
		{
			for(int i = 0; i < _lines.Length && i < _stats._dailyRewards.Length; i++)
			{
				_lines[i].Init(_stats._dailyRewards[i]);
			}
		}
		
		public int GetCurrentReward(float prevProgress, float currentProgress)
		{
			int prevCoins = 0;
			int coins = 0;
			for(int i = 0; i < _stats._dailyRewards.Length; i++)
			{
				if(prevProgress >= _stats._dailyRewards[i].Percentage
					|| i == _stats._dailyRewards.Length - 1)
				{
					prevCoins = _stats._dailyRewards[i].Coins;
					break;
				}
			}
			for(int i = 0; i < _stats._dailyRewards.Length; i++)
			{
				if(currentProgress >= _stats._dailyRewards[i].Percentage
					|| i == _stats._dailyRewards.Length - 1)
				{
					coins = _stats._dailyRewards[i].Coins;
					break;
				}
			}
			return coins - prevCoins;
		}
		
		public void TryUpdateDailyProgress(float progress)
		{
			if(progress > _stats._dailyProcentage)
			{
				float prevProgress = _stats._dailyProcentage;
				_stats._dailyProcentage = progress;
				_coins.AddEarnedCoins(GetCurrentReward(prevProgress, _stats._dailyProcentage));
			}
		}
		
		public bool IsCurrentReward(DailyReward reward)
		{
			if(_stats.DailyProcentage >= reward.Percentage)
			{
				int currentIndex = -1;
				for(int i = 0; i < _stats._dailyRewards.Length; i++)
				{
					if(reward.Equals(_stats._dailyRewards[i]))
					{
						currentIndex = i;
					}
				}
				
				if(currentIndex == 0 
					|| _stats.DailyProcentage < _stats._dailyRewards[currentIndex - 1].Percentage)
				{
					return true;
				}
			}
			return false;
		}
	    
		public void PlayChallange()
		{
			if(_stats._dailyAttempts > 0)
			{
				_levelManager.StartDailyLevel();
				_stats._dailyAttempts--;
				_player.Init();
				_starter.BeginPlay();
				_challengeCnavas.SetActive(false);
			}
		}
	}
	
	[System.Serializable]
	public struct DailyReward
	{
		public Vector2Int MinMaxHumans;
		public float Percentage;
		public int Coins;
		[HideInInspector] public int Humans;
	}
}