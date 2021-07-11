using System.Collections;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MapGeneration : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _platformAmount = 10;
		[SerializeField] private Platform _lastPlatform;
		
		[SerializeField, HideInInspector] private PlatformsHolder _prefabs;
		[SerializeField] private LetterPickup LetterPrefab;
		[SerializeField] private GameObject ManikinPrefab;
		
		//------METHODS
		public void UpdateReferences()
		{
			_prefabs = gameObject.GetComponentInChildren<PlatformsHolder>();
		}
		
		private void Start()
		{
			StartCoroutine(Generate());
		}
		
		private IEnumerator Generate()
		{
			for(int i = 0; i < _platformAmount; i++)
			{
				SpawnPlatform(i % 2 == 0 ? true : false);
				yield return null;
			}
			SpawnPlatform(_prefabs.GetFinishPlatform());
		}
		
		private void SpawnPlatform(Platform prefab)
		{
			if(_lastPlatform == null)
				return;
			
			Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
			newPlatform.Init(LetterPrefab, ManikinPrefab);
			_lastPlatform = newPlatform;
		}
		
		private void SpawnPlatform(bool emptyPlatform)
		{
			if(_lastPlatform == null)
				return;
				
			Platform prefab = null;
			if(emptyPlatform)
				prefab = _prefabs.GetEmptyPlatform();
			else
				prefab = _prefabs.GetObstaclePlatform();
			
			Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
			newPlatform.Init(LetterPrefab, ManikinPrefab);
			_lastPlatform = newPlatform;
		}
	}
}