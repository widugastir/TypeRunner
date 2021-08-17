using System.Collections.Generic;
using UnityEngine;
using SoundSteppe.RefSystem;

namespace TypeRunner
{
	public class Platform : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[Header("Settings")]
		//[SerializeField] private bool _canBeMirrored = true;
		[SerializeField] private int _generationLevel;
		[SerializeField] private int _manikinAmount;
		[SerializeField] private int _letterAmount;
		[SerializeField] private RespawnPoint _respawnPoint;
		[SerializeField] private List<E_LetterType> _requiredLetters;
		
		[Header("References")]
		public Transform ConnectionPoint;
		[SerializeField] private Coin[] _coins;
		[SerializeField] private List<Transform> _letterSpawnPos;
		[SerializeField] private List<Transform> _manikinSpawnPos;
		private MapGenerationLevels _generator;
		[SerializeField] private float _dailyStickmanMultiplier = 0.5f;
		private float _stickmanMultiplier = 1f;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)  
		{
			if(sceneObject == false)
			{
				_coins = GetComponentsInChildren<Coin>();
			}
		}
		
		public void Init(
			MapGenerationLevels generator, 
			Transform parent, 
			CoinManager coinManager,
			bool daily = false)
		{
			//if(_canBeMirrored)
			//{
			//	if(Random.value >= 0.5f)
			//	{
			//		Vector3 newScale = transform.localScale;
			//		newScale.x *= -1f;
			//		transform.localScale = newScale;
			//	}
			//}
			gameObject.SetActive(true);
			transform.SetParent(parent);
			
			if(daily)
				_stickmanMultiplier = _dailyStickmanMultiplier;
			else
				_stickmanMultiplier = 1f;
				
			_generator = generator;
			
			SpawnLetters();
			SpawnMankins();
			for(int i = 0; i < _coins.Length; i++)
			{
				_coins[i].Init(coinManager);
			}
		}
		
		private void SpawnLetters()
		{
			int index = 0;
			for(int i = 0; i < _letterAmount; i++)
			{
				if(_letterSpawnPos == null || _letterSpawnPos.Count == 0)
					break;
					
				index = Random.Range(0, _letterSpawnPos.Count);
				var letter = _generator.SpawnLetter(_letterSpawnPos[index].position);
				
				if(_requiredLetters.Count > 0)
				{
					letter.Init(_requiredLetters[0]);
					_requiredLetters.RemoveAt(0);
				}
				else 
				{
					letter.Init();
				}
				
				_letterSpawnPos.RemoveAt(index);
			}
		}
		
		private void SpawnMankins()
		{
			int index = 0;
			for(int i = 0; i < _manikinAmount * _stickmanMultiplier; i++)
			{
				if(_manikinSpawnPos.Count == 0)
					break;
					
				index = Random.Range(0, _manikinSpawnPos.Count);
				var man = _generator.SpawnManikin(_manikinSpawnPos[index].position);
				_manikinSpawnPos.RemoveAt(index);
			}
		}
	}
}