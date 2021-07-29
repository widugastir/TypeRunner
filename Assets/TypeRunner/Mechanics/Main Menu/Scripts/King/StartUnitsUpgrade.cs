using SoundSteppe.RefSystem;
using UnityEngine;
using System;

namespace TypeRunner
{
	public class StartUnitsUpgrade : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _maxLevel = 10;
		[SerializeField] private int _baseCost = 50;
		[SerializeField] private float _costMultiplier = 0.25f;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_coins = FindObjectOfType<CoinManager>(true);
			}
		}
		
		public int GetUpgradeCost() => _baseCost + (int)((float)(_stats.StartUnitsLevel - 1) * _baseCost * _costMultiplier);
		
		public bool CanUpgrade()
		{
			if(_stats.StartUnitsLevel >= _maxLevel
				|| _stats.Coins < GetUpgradeCost())
				return false;
			return true;
		}
		
		public bool TryUpgrade()
		{
			if(_stats.StartUnitsLevel >= _maxLevel)
				return false;
				
			if(_coins.TrySpend(GetUpgradeCost()))
			{
				_stats.StartUnitsLevel++;
				return true;
			}
			return false;
		}
	}
}