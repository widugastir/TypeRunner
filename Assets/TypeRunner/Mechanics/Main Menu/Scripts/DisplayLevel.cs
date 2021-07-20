using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class DisplayLevel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private TMP_Text _textUI;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_stats = FindObjectOfType<PlayerStats>(true);
		}
		
		private void OnEnable()
		{
			PlayerStats.OnCoinChange += CoinChange;
			SaveSystem.OnEndLoad += UpdateUI;
			UpdateUI();
		}
		
		private void OnDisable()
		{
			PlayerStats.OnCoinChange -= CoinChange;
			SaveSystem.OnEndLoad += UpdateUI;
		}
		
		private void UpdateUI()
		{
			_textUI.text = "Level: " + _stats.CurrentLevel.ToString();
		}
		
		private void CoinChange(int coins)
		{
			UpdateUI();
		}
	}
}