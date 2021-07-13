using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class LevelManager : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject _inGameUI;
		//[SerializeField] private int _victoryBonus = 15;
		//[SerializeField, HideInInspector] private GlassPlacer _wall;
		//[SerializeField, HideInInspector] private Towers _towers;
		//[SerializeField, HideInInspector] private PlayerStats _stats;
		//[SerializeField, HideInInspector] private CoinManager _coins;
		//public static event System.Action<bool> OnLevelEnd;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				//_wall = FindObjectOfType<GlassPlacer>(true);
				//_towers = FindObjectOfType<Towers>(true);
				//_stats = FindObjectOfType<PlayerStats>(true);
				//_coins = FindObjectOfType<CoinManager>(true);
			}
		}
		
		private void OnEnable()
		{
			//MainTower.OnDestroy += OnTowerDestroy;
		}
		
		private void OnDisable()
		{
			//MainTower.OnDestroy -= OnTowerDestroy;
		}
		
		//private void OnTowerDestroy(MainTower tower)
		//{
		//	if(tower.Owner == E_Owner.player)
		//		OnLevelEnd?.Invoke(false); // Lose
		//	else
		//	{
		//		_stats.CurrentLevel++;
		//		_coins.AddEarnedCoins(_victoryBonus);
		//		OnLevelEnd?.Invoke(true); // Victory
		//	}
		//	LevelEnd();
		//}
		
		private void LevelEnd()
		{
			Time.timeScale = 0f;
			ResetLevel();
		}
		
		private void ResetLevel()
		{
			_inGameUI.SetActive(false);
			//_wall.Reset();
			//_towers.Reset();
		}
	}
}