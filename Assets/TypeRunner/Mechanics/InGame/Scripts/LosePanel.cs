using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class LosePanel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private Button _reviveButton;
		[SerializeField] private Button _mainMenuButton;
		[SerializeField, HideInInspector] private GameStarter _startet;
		[SerializeField, HideInInspector] private GroupCenter _groupCenter;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private MapGeneration _generator;
		[SerializeField, HideInInspector] private MapMovement _map;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_startet = FindObjectOfType<GameStarter>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_coins = FindObjectOfType<CoinManager>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_groupCenter = FindObjectOfType<GroupCenter>(true);
				_generator = FindObjectOfType<MapGeneration>(true);
				_map = FindObjectOfType<MapMovement>(true);
			}
		}
		
		private void OnEnable()
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			if(_stats.Coins >= _stats.ReviveCost)
			{
				_reviveButton.gameObject.SetActive(true);
				_mainMenuButton.gameObject.SetActive(false);
			}
			else
			{
				_reviveButton.gameObject.SetActive(false);
				_mainMenuButton.gameObject.SetActive(true);
			}
		}
		
		public void Revive()
		{
			if(_coins.TrySpend(_stats.ReviveCost))
			{
				_stats.ReviveCount++;
				_groupCenter.Reset();
				_player.ResetMansParent();
				_startet.BeginPlay();
				_map.PushBack();
				_generator.DestroyRagdolls();
			}
		}
	}
}