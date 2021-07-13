using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class LevelDisplay : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private TMP_Text _levelText;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_stats = FindObjectOfType<PlayerStats>(true);
		}
		
		private void OnEnable()
		{
			UpdateUI();
			PlayerStats.OnLevelChange += LevelChange;
			SaveSystem.OnEndLoad += UpdateUI;
		}
		
		private void OnDisable()
		{
			PlayerStats.OnLevelChange -= LevelChange;
			SaveSystem.OnEndLoad += UpdateUI;
		}
		
		private void UpdateUI()
		{
			_levelText.text = "LEVEL " + _stats.CurrentLevel.ToString();
		}
		
		private void LevelChange(int level)
		{
			UpdateUI();
		}
	}
}