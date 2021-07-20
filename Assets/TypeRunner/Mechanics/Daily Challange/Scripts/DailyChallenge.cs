using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class DailyChallenge : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _attemptsPerDay = 3;
		[SerializeField] private int _dailyUpdateHour = 12;
		[SerializeField] private GameObject _challengeCnavas;
		[SerializeField] private TMP_Text _attemptsText;
		[SerializeField, HideInInspector] private DailyLine[] _lines;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private GameStarter _starter;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
	    
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_starter = FindObjectOfType<GameStarter>(true);
				_levelManager = FindObjectOfType<LevelManager>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_lines = GetComponentsInChildren<DailyLine>(true);
				_coins = FindObjectOfType<CoinManager>(true);
			}
		}
		
		private void OnEnable()
		{
			SaveSystem.OnEndLoad += TryUpdateAttempts;
		}
		
		private void OnDisable()
		{
			SaveSystem.OnEndLoad -= TryUpdateAttempts;
		}
		
		public void EnableCanvas()
		{
			UpdateUI();
			_challengeCnavas.SetActive(true);
		}
		
		private void UpdateUI()
		{
			_attemptsText.text = "Attempts: " + _stats._dailyAttempts.ToString();
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
				
				DailyReward[] rewards = _stats._dailyRewards;
				for(int i = 0; i < rewards.Length; i++)
				{
					rewards[i].Humans = Random.Range(rewards[i].MinMaxHumans.x, rewards[i].MinMaxHumans.y);;
				}
			}
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
		
		public DailyReward GetCurrentReward()
		{
			for(int i = 0; i < _stats._dailyRewards.Length; i++)
			{
				if(_stats._dailyProcentage >= _stats._dailyRewards[i].Percentage
					&&(i == _stats._dailyRewards.Length - 1
					|| _stats._dailyProcentage < _stats._dailyRewards[i + 1].Percentage))
				{
					return _stats._dailyRewards[i];
				}
			}
			return default;
		}
		
		public void TryUpdateDailyProgress(float progress)
		{
			if(progress > _stats._dailyProcentage)
			{
				_stats._dailyProcentage = progress;
				_coins.AddEarnedCoins(GetCurrentReward().Coins);
			}
		}
		
		public bool IsCurrentReward(DailyReward reward)
		{
			if(_stats.DailyProcentage >= reward.Percentage)
			{
				for(int i = 0; i < _stats._dailyRewards.Length; i++)
				{
					if(reward.Equals(_stats._dailyRewards[i])
						&&(i == _stats._dailyRewards.Length - 1
						|| reward.Percentage < _stats._dailyRewards[i + 1].Percentage))
					{
						return true;
					}
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