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
		
		[Saveable] public int _coins = 0;
		[Saveable] public int _currentLevel = 0;
		[Saveable] public int _startUnitsLevel = 1;
		[Saveable] public int _incomeLevel = 1;
		[HideInInspector] public int EarnedCoins {get; set;} = 0;
		
		[Saveable] public DateTime LastLogin;
		
		public static Action<int> OnCoinChange;
		public static Action<int> OnLevelChange;
		public static Action<int> OnStartUnitChange;
		public static Action<int> OnIncomeLevelChange;
	}
}