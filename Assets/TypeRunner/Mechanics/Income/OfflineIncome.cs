using SoundSteppe.RefSystem;
using UnityEngine;
using System;

namespace TypeRunner
{
	public class OfflineIncome : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _costPerLevel = 100;
		[SerializeField] private int _coinsPerMinute;
		[SerializeField] private int _reward;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private IncomePanel _panel;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_coins = FindObjectOfType<CoinManager>(true);
				_panel = FindObjectOfType<IncomePanel>(true);
			}
		}
		
		private void OnEnable() 
		{ 
			SaveSystem.OnBeginSave += OnBeginSave; 
			SaveSystem.OnEndLoad += OnEndLoad; 
		}
		
		private void OnDisable() 
		{ 
			SaveSystem.OnBeginSave += OnBeginSave; 
			SaveSystem.OnEndLoad -= OnEndLoad; 
		}
		
		public int GetUpgradeCost() => _stats.IncomeLevel * _costPerLevel;
		
		private void OnEndLoad()
		{
			TimeSpan span;
			if(_stats.LastLogin != default)
			{
				span = DateTime.Now - _stats.LastLogin;
			}
			else return;
			
			int minutes = (int)span.TotalMinutes;
			_reward = minutes * _coinsPerMinute * _stats.IncomeLevel;
			
			if(_reward >= 60)
			{
				_panel.Enable(_reward);
			}
		}
		
		private void OnBeginSave()
		{
			_stats.LastLogin = DateTime.Now;
		}
		
		public bool TryUpgrade()
		{
			if(_coins.TrySpend(GetUpgradeCost()))
			{
				_stats.IncomeLevel++;
				return true;
			}
			return false;
		}
	}
}