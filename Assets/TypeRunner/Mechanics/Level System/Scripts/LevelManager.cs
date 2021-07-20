using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class LevelManager : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject[] _disableOnFinish;
		[SerializeField] private int _baseCoinsPerVictory = 10;
		[SerializeField, HideInInspector] private MapGeneration _map;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private CameraResetter _cameraResetter;
		[SerializeField, HideInInspector] private LetterWriteSystem _letterSystem;
		public static event System.Action<bool> OnLevelEnd;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_coins = FindObjectOfType<CoinManager>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_cameraResetter = FindObjectOfType<CameraResetter>(true);
				_map = FindObjectOfType<MapGeneration>(true);
				_letterSystem = FindObjectOfType<LetterWriteSystem>(true);
			}
		}
		
		public void StartDailyLevel()
		{
			_letterSystem.Reset();
			_map.ResetToDaily();
		}
		
		public void StartLevel()
		{
			_letterSystem.Reset();
		}
		
		public void FinishLevel(bool victory, float coinsMultiplier = 1f)
		{
			if(victory)
			{
				_stats.CurrentLevel++;
				_coins.AddEarnedCoins((int)((float)_baseCoinsPerVictory * coinsMultiplier));
			}
			LevelEnd();
			OnLevelEnd?.Invoke(victory);
		}
		
		private void LevelEnd()
		{
			Time.timeScale = 0f;
			ResetLevel();
		}
		
		private void ResetLevel()
		{
			foreach(var p in _disableOnFinish)
				p.SetActive(false);
			_map.Reset();
			_player.Reset();
			_stats.Reset();
			_cameraResetter.Reset();
		}
	}
}