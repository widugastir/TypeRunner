using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class Platform : MonoBehaviour
	{
		//------FIELDS
		[Header("Settings")]
		[SerializeField] private int LetterAmount;
		[SerializeField] private int ManikinAmount;
		
		[Header("References")]
		public Transform ConnectionPoint;
		[SerializeField] private List<Transform> LetterSpawnPos;
		[SerializeField] private List<Transform> ManikinSpawnPos;
		
		//------METHODS
		public void Init(GameObject letterPrefab, GameObject manikinPrefab)
		{
			SpawnLetters(letterPrefab);
			SpawnMankins(manikinPrefab);
		}
		
		private void SpawnLetters(GameObject letterPrefab)
		{
			int index = 0;
			for(int i = 0; i < LetterAmount; i++)
			{
				if(LetterSpawnPos.Count == 0)
					break;
					
				index = Random.Range(0, LetterSpawnPos.Count);
				Instantiate(letterPrefab, LetterSpawnPos[index].position, Quaternion.identity);
				LetterSpawnPos.RemoveAt(index);
			}
		}
		
		private void SpawnMankins(GameObject manikinPrefab)
		{
			int index = 0;
			for(int i = 0; i < ManikinAmount; i++)
			{
				if(ManikinSpawnPos.Count == 0)
					break;
					
				index = Random.Range(0, ManikinSpawnPos.Count);
				Instantiate(manikinPrefab, ManikinSpawnPos[index].position, Quaternion.identity);
				ManikinSpawnPos.RemoveAt(index);
			}
		}
	}
}