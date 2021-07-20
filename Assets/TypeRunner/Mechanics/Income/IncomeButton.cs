using SoundSteppe.RefSystem;
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
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_income = FindObjectOfType<Income>(true);
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
			if(_income.TryUpgrade())
				UpdateUI();
		}
		
		private void UpdateUI()
		{
			_levelText.text = _stats.IncomeLevel.ToString();
			_costText.text = _income.GetUpgradeCost().ToString();
		}
	}
}