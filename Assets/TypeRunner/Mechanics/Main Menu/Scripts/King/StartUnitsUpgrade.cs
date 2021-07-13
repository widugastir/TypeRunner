using SoundSteppe.RefSystem;
using UnityEngine;
using System;

namespace TypeRunner
{
	public class StartUnitsUpgrade : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _costPerLevel = 150;
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
		
		public int GetUpgradeCost() => _stats.StartUnitsLevel * _costPerLevel;
		
		public bool TryUpgrade()
		{
			if(_coins.TrySpend(GetUpgradeCost()))
			{
				_stats.StartUnitsLevel++;
				return true;
			}
			return false;
		}
	}
}