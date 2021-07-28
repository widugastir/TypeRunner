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
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_coins = FindObjectOfType<CoinManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_levelManager = FindObjectOfType<LevelManager>(true);
			}
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
		
		public void Disable(bool restartLevel = false)
		{
			if(restartLevel)
			{
				//_previewCamera.SetActive(true);
				//_coins.EarnedToCurrent();
				_losePanel.SetActive(false);
				_victoryPanel.SetActive(false);
				//_levelManager.FinishLevel();
				_player.Init(true, 3f);
			}
			else
			{
				_previewCamera.SetActive(true);
				_coins.EarnedToCurrent();
				_losePanel.SetActive(false);
				_victoryPanel.SetActive(false);
				_levelManager.FinishLevel();
				_player.Init();
			}
		}
	}
}