﻿using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class StartUnitsUpgradeButton : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private TMP_Text _levelText;
		[SerializeField] private TMP_Text _costText;
		[SerializeField, HideInInspector] private StartUnitsUpgrade _upgrade;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_upgrade = FindObjectOfType<StartUnitsUpgrade>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
			}
		}
		
		private void OnEnable() { SaveSystem.OnEndLoad += OnEndLoad; }
		private void OnDisable() { SaveSystem.OnEndLoad += OnEndLoad; }
		
		private void OnEndLoad()
		{
			UpdateUI();
		}
		
		public void OnPress()
		{
			if(_upgrade.TryUpgrade())
				UpdateUI();
		}
		
		private void UpdateUI()
		{
			_levelText.text = _stats.StartUnitsLevel.ToString();
			_costText.text = _upgrade.GetUpgradeCost().ToString();
		}
	}
}