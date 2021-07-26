using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class IncomeButton : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private TMP_Text _levelText;
		[SerializeField] private TMP_Text _costText;
		[SerializeField, HideInInspector] private Income _income;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private Button _button;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_income = FindObjectOfType<Income>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
			}
			else
			{
				_button = GetComponentInChildren<Button>(true);
			}
		}
		
		private void OnEnable() 
		{ 
			SaveSystem.OnEndLoad += OnEndLoad; 
			PlayerStats.OnCoinChange += CoinChanged;
			UpdateUI();
		}
		
		private void OnDisable() 
		{ 
			SaveSystem.OnEndLoad -= OnEndLoad; 
			PlayerStats.OnCoinChange -= CoinChanged;
		}
		
		private void OnEndLoad()
		{
			UpdateUI();
		}
		
		public void OnPress()
		{
			if(_income.TryUpgrade())
				UpdateUI();
		}
		
		private void CoinChanged(int coins)
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			if(_stats.Coins >= _income.GetUpgradeCost())
			{
				_button.interactable = true;
			}
			else
			{
				_button.interactable = false;
			}
			_levelText.text = _stats.IncomeLevel.ToString();
			_costText.text = _income.GetUpgradeCost().ToString();
		}
	}
}