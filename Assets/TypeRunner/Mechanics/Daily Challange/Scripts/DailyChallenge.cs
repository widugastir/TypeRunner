using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class DailyChallenge : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _attemptsPerDay = 3;
		[SerializeField] private int _dailyUpdateHour = 12;
		//[SerializeField] private DailyReward[] _rewards;
		[SerializeField, HideInInspector] private DailyLine[] _lines;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private PlayerStats _stats;
	    
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_levelManager = FindObjectOfType<LevelManager>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_lines = GetComponentsInChildren<DailyLine>(true);
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
		
		private void TryUpdateAttempts()
		{
			System.DateTime now = System.DateTime.Now;
			System.DateTime unlockDate = new System.DateTime(now.Year, now.Month, now.Day, _dailyUpdateHour, 0, 0);
			
			if(_stats.LastLogin < unlockDate
				&& now.Hour >= _dailyUpdateHour
				|| _stats.LastLogin == default)
			{
				print(12344);
				_stats._dailyAttempts = _attemptsPerDay;
				_stats._dailyCategory = 0;
				
				DailyReward[] rewards = _stats._dailyRewards;
				for(int i = 0; i < rewards.Length; i++)
				{
					rewards[i].Humans = Random.Range(rewards[i].MinMaxHumans.x, rewards[i].MinMaxHumans.y);;
				}
			}
			InitLines();
		}
		
		private void InitLines()
		{
			for(int i = 0; i < _lines.Length && i < _stats._dailyRewards.Length; i++)
			{
				_lines[i].Init(_stats._dailyRewards[i]);
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
						|| reward.Percentage <= _stats._dailyRewards[i + 1].Percentage))
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