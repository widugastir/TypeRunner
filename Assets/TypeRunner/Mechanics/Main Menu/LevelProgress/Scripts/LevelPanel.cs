using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class LevelPanel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private LevelChunk[] _chunks;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_chunks = GetComponentsInChildren<LevelChunk>(true);
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
			int level = _stats.CurrentLevel;
			int mtpl = level / 9;
			int bonusLevel = level - mtpl * 8;
			
			for(int i = 0; i < _chunks.Length; i++)
			{
				if(i < bonusLevel)
					_chunks[i].Highlight();
				else
					_chunks[i].Downlight();
			}
		}
		
		private void LevelChange(int level)
		{
			UpdateUI();
		}
	}
}