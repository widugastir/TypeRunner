using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class EarnedCoinsDisplay : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private bool _writePlus = true;
		[SerializeField] private TMP_Text _labelText;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_stats = FindObjectOfType<PlayerStats>(true);
		}
		
		private void OnEnable()
		{
			PlayerStats.OnEarnedCoinsChange += OnCoinChange;
			UpdateUI();
		}
		
		private void OnDisable()
		{
			PlayerStats.OnEarnedCoinsChange -= OnCoinChange;
		}
		
		private void OnCoinChange(int earned)
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			_labelText.text = (_writePlus? "+" : "") + (_stats.EarnedCoins).ToString("0");
		}
	}
}