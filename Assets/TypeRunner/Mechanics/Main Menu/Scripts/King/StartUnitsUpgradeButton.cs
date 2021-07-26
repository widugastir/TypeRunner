using SoundSteppe.RefSystem;
using UnityEngine.UI;
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
		[SerializeField, HideInInspector] private Button _button;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_upgrade = FindObjectOfType<StartUnitsUpgrade>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
			}
			else
			{
				_button = GetComponentInChildren<Button>(true);
			}
		}
		
		private void OnEnable() 
		{ 
			PlayerStats.OnCoinChange += CoinChanged;
			SaveSystem.OnEndLoad += OnEndLoad; 
			UpdateUI();
		}
		
		private void OnDisable() 
		{ 
			PlayerStats.OnCoinChange -= CoinChanged;
			SaveSystem.OnEndLoad -= OnEndLoad; 
		}
		
		private void OnEndLoad()
		{
			UpdateUI();
		}
		
		public void OnPress()
		{
			if(_upgrade.TryUpgrade())
				UpdateUI();
		}
		
		private void CoinChanged(int coins)
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			if(_stats.Coins >= _upgrade.GetUpgradeCost())
			{
				_button.interactable = true;
			}
			else
			{
				_button.interactable = false;
			}
			_levelText.text = _stats.StartUnitsLevel.ToString();
			_costText.text = _upgrade.GetUpgradeCost().ToString();
		}
	}
}