using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class Platform : MonoBehaviour
	{
		//------FIELDS
		[Header("Settings")]
		[SerializeField] private int _generationLevel;
		[SerializeField] private int _manikinAmount;
		[SerializeField] private int _letterAmount;
		[SerializeField] private List<E_LetterType> _requiredLetters;
		
		[Header("References")]
		public Transform ConnectionPoint;
		[SerializeField] private List<Transform> _letterSpawnPos;
		[SerializeField] private List<Transform> _manikinSpawnPos;
		
		//------METHODS
		public void Init(LetterPickup letterPrefab, GameObject manikinPrefab)
		{
			SpawnLetters(letterPrefab);
			SpawnMankins(manikinPrefab);
		}
		
		private void SpawnLetters(LetterPickup letterPrefab)
		{
			int index = 0;
			for(int i = 0; i < _letterAmount; i++)
			{
				if(_letterSpawnPos.Count == 0)
					break;
					
				index = Random.Range(0, _letterSpawnPos.Count);
				var letter = Instantiate(letterPrefab, _letterSpawnPos[index].position, Quaternion.identity);
				
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
		
		private void SpawnMankins(GameObject manikinPrefab)
		{
			int index = 0;
			for(int i = 0; i < _manikinAmount; i++)
			{
				if(_manikinSpawnPos.Count == 0)
					break;
					
				index = Random.Range(0, _manikinSpawnPos.Count);
				Instantiate(manikinPrefab, _manikinSpawnPos[index].position, Quaternion.identity);
				_manikinSpawnPos.RemoveAt(index);
			}
		}
	}
}