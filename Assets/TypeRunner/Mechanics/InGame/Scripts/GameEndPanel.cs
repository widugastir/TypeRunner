using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class GameEndPanel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject _previewCamera;
		[SerializeField] private GameObject _victoryPanel;
		[SerializeField] private GameObject _losePanel;
		[SerializeField] private GameObject _bonusSkinProgress;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerController _player;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_coins = FindObjectOfType<CoinManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
			}
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
			if(victory)
			{
				_victoryPanel.SetActive(true);
				//_bonusSkinProgress.SetActive(true);
			}
			else
			{
				_losePanel.SetActive(true);
				//_bonusSkinProgress.SetActive(false);
			}
		}
		
		public void Disable()
		{
			_previewCamera.SetActive(true);
			_coins.EarnedToCurrent();
			_losePanel.SetActive(false);
			_victoryPanel.SetActive(false);
			_player.Init();
		}
	}
}