using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class EarnedCoinsDisplay : MonoBehaviour, INeedReference
	{
		//------FIELDS
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
			_labelText.text = "+" + (_stats.EarnedCoins * _stats.SuccessfulMultiplier).ToString("0");
		}
	}
}