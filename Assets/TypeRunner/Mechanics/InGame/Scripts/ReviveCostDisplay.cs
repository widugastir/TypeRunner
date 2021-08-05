using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class ReviveCostDisplay : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField] private TMP_Text _text;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
			}
		}
		
		private void OnEnable()
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			_text.text = "REVIVE: " + _stats.ReviveCost.ToString();
		}
	}
}