using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class CustomLetterGenerator : MonoBehaviour
	{
		[SerializeField] private List<E_LetterType> _requiredLetters;
		[SerializeField] private List<Transform> _letterSpawnPos;
		[SerializeField] private int _lettersAmount;
		
		public void Generate(MapGenerationLevels generator)
		{
			int index = 0;
			for(int i = 0; i < _lettersAmount; i++)
			{
				if(_letterSpawnPos.Count == 0)
					break;
						
				index = Random.Range(0, _letterSpawnPos.Count);
				var letter = generator.SpawnLetter(_letterSpawnPos[index].position);
					
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
	}
}