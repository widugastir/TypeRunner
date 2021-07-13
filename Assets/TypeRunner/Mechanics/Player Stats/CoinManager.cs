﻿using SoundSteppe.RefSystem;
using UnityEngine;
using System;

namespace TypeRunner
{
	public class CoinManager : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_stats = FindObjectOfType<PlayerStats>(true);
		}
		
		public bool TrySpend(int coins)
		{
			if(_stats.Coins < coins)
				return false;
			
			_stats.Coins -= coins;
			return true;
		}
		
		public void AddCoins(int coins)
		{
			_stats.Coins += coins;
		}
		
		public void AddEarnedCoins(int coins)
		{
			_stats.EarnedCoins += coins;
		}
		
		public void EarnedToCurrent()
		{
			AddCoins(_stats.EarnedCoins);
			_stats.EarnedCoins = 0;
		}
	}
}