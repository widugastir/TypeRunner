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
		private MapGeneration _generator;
		
		//------METHODS
		public void Init(MapGeneration generator)
		{
			_generator = generator;
			SpawnLetters();
			SpawnMankins();
		}
		
		private void SpawnLetters()
		{
			int index = 0;
			for(int i = 0; i < _letterAmount; i++)
			{
				if(_letterSpawnPos.Count == 0)
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
			for(int i = 0; i < _manikinAmount; i++)
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