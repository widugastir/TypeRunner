using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class GameEndPanel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject _menuPanel;
		[SerializeField] private GameObject _dailyPanel;
		[SerializeField] private GameObject _previewCamera;
		[SerializeField] private GameObject _victoryPanel;
		[SerializeField] private GameObject _losePanel;
		[SerializeField] private GameObject _bonusSkinProgress;
		[SerializeField] private LevelRoulette _roulette;
		[SerializeField] private GameObject _buttonContinue;
		[SerializeField, HideInInspector] private MapGenerationLevels _generator;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private DailyChallenge _dailyChallange;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_generator = FindObjectOfType<MapGenerationLevels>(true);
				_coins = FindObjectOfType<CoinManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_levelManager = FindObjectOfType<LevelManager>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_dailyChallange = FindObjectOfType<DailyChallenge>(true);
			}
		}
		
		public void Enable(bool victory, bool enableSkinProgress)
		{
			if(victory)
			{
				_buttonContinue.SetActive(true);
				_victoryPanel.SetActive(true);
				_roulette.Disable();
				
				if(_levelManager._isDailyLevel == false 
					&&_roulette.Enable())
				{
					_buttonContinue.SetActive(false);
				}
				
				if(enableSkinProgress)	_bonusSkinProgress.SetActive(true);
				else					_bonusSkinProgress.SetActive(false);
			}
			else
			{
				_losePanel.SetActive(true);
			}
		}
		
		public void Disable(bool restartLevel = false)
		{
			if(_stats.SkinBonusProgress >= 1f)
			{
				_stats.SkinBonusProgress = 0f;
			}
			if(restartLevel)
			{
				_losePanel.SetActive(false);
				_victoryPanel.SetActive(false);
				_player.Init(true, 3f);
			}
			else
			{
				if(_levelManager._isDailyLevel)
					_dailyChallange.EnableCanvas();
				else
					_menuPanel.SetActive(true);
				_previewCamera.SetActive(true);
				_coins.EarnedToCurrent();
				_losePanel.SetActive(false);
				_victoryPanel.SetActive(false);
				_levelManager.FinishLevel();
				_generator.Generate();
				_player.Init();
			}
		}
	}
}