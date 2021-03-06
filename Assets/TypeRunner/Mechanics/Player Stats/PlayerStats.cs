using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;

namespace TypeRunner
{
	public class PlayerStats : MonoBehaviour
	{
		//------FIELDS
		public int Coins
		{
			get { return _coins; }
			set 
			{ 
				_coins = value; 
				OnCoinChange?.Invoke(_coins);
			}
		}
		
		public int CurrentLevel
		{
			get { return _currentLevel; }
			set 
			{ 
				_currentLevel = value;
				OnLevelChange?.Invoke(_currentLevel);
			}
		}
		
		public int StartUnitsLevel
		{
			get { return _startUnitsLevel; }
			set 
			{ 
				_startUnitsLevel = value;
				OnStartUnitChange?.Invoke(_startUnitsLevel);
			}
		}
		
		public int IncomeLevel
		{
			get { return _incomeLevel; }
			set 
			{ 
				_incomeLevel = value;
				OnIncomeLevelChange?.Invoke(_incomeLevel);
			}
		}
		
		public float DailyProcentage
		{
			get { return _dailyProcentage; }
			set 
			{ 
				_dailyProcentage = value;
				OnDailyChange?.Invoke(_dailyProcentage);
			}
		}
		
		public float SkinBonusProgress
		{
			get { return _skinBonusProgress; }
			set 
			{ 
				PrevSkinProgress = _skinBonusProgress;
				_skinBonusProgress = value;
				OnSkinProgressChange?.Invoke(_skinBonusProgress);
			}
		}
		
		[Saveable, HideInInspector] public float _rouletteRotate = -1f;
		[Saveable, HideInInspector] public Color _playerColor;
		[SerializeField] private int _reviveCost = 100;
		public int ReviveCost => Mathf.Max(_reviveCost, (_reviveCost * ReviveCount) * 2);
		public int ReviveCount = 0;
		public int MaxStickmans = 136;
		public float _coinsMultiplier = 1f;
		[Saveable] public int _coins = 0;
		[Saveable] public int LevelPlayCount = 0;
		[Saveable] public int _currentLevel = 0;
		[Saveable] public int _startUnitsLevel = 1;
		[Saveable] public int _incomeLevel = 1;
		[Saveable] public int _dailyAttempts = 3;
		[Saveable] public int _dailyUpdatesCount = 0;
		[Saveable] public int _dailyCategory = 0;
		[Saveable] public float _dailyProcentage = 0f;
		[Saveable] public float _skinBonusProgress = 0f;
		[Saveable] public bool _freeColorRoulette = true;
		[Saveable] public bool _tutorialAvailable = true;
		[Saveable] public int _bonusSkinGained = 0;
		[Saveable] public int _currentLevelIndex = 0;

		public int _successfulWord = 0;
		public float SuccessfulMultiplier
		{
			get 
			{
				//return (float)System.Math.Round(1f + 0.3f * (float)(_successfulWord - 1), 1);
				return (float)System.Math.Round((double)Mathf.Pow(1.3f, _successfulWord), 1);
				//return (float)System.Math.Round((double)_successfulWord * 0.5D, 2);
			}
		}

		public int _earnedCoins = 0;
		[HideInInspector] public int EarnedCoins 
		{
			get
			{
				return _earnedCoins;
			}
			set
			{
				_earnedCoins = value;
				OnEarnedCoinsChange?.Invoke(_earnedCoins);
			}
		}
		[HideInInspector] public float PrevSkinProgress {get; set;} = 0f;

		[Saveable] public SkinType _playerSkin = SkinType.S1;
		[Saveable] public List<SkinType> PurchasedSkins;

		[ReorderableList] [Saveable] public DailyReward[] _dailyRewards;
		[Saveable] public DateTime LastLogin;
		[Saveable] public DateTime LastDailyPlayersUpdate;
		
		public static Action<int> OnEarnedCoinsChange;
		public static Action<int> OnCoinChange;
		public static Action<int> OnLevelChange;
		public static Action<int> OnStartUnitChange;
		public static Action<int> OnIncomeLevelChange;
		public static Action<float> OnDailyChange;
		public static event Action<float> OnSkinProgressChange;

		public void Reset()
		{
			ReviveCount = 0;
		}
		
		private void Start()
		{
			if(PurchasedSkins.Contains(SkinType.S1) == false)
			{
				PurchasedSkins.Add(SkinType.S1);
			}
		}
	}
}