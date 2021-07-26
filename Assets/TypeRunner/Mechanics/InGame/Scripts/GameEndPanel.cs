using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class GameEndPanel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject _previewCamera;
		[SerializeField] private TMP_Text _labelText;
		[SerializeField] private GameObject _panel;
		[SerializeField] private GameObject _bonusSkinProgress;
		[SerializeField, HideInInspector] private CoinManager _coins;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_coins = FindObjectOfType<CoinManager>(true);
		}
		
		private void OnEnable()
		{
			LevelManager.OnLevelEnd += Enable;
		}
		
		private void OnDisable()
		{
			LevelManager.OnLevelEnd -= Enable;
		}
		
		public void Enable(bool victory)
		{
			_panel.SetActive(true);
			if(victory)
			{
				_bonusSkinProgress.SetActive(true);
				_labelText.text = "Victory!";
			}
			else
			{
				_bonusSkinProgress.SetActive(false);
				_labelText.text = "Lose!";
			}
		}
		
		public void Disable()
		{
			_previewCamera.SetActive(true);
			_coins.EarnedToCurrent();
			_panel.SetActive(false);
		}
	}
}