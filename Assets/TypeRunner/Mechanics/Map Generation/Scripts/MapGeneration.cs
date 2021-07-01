using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class MapGeneration : MonoBehaviour
	{
		[SerializeField] private int _platformAmount = 10;
		[SerializeField] private Platform _lastPlatform;
		
		[SerializeField, HideInInspector] private PlatformsHolder _prefabs;
		
		//------METHODS
		[Button]
		private void UpdateReferences()
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
				SpawnPlatform();
				yield return null;
			}
		}
		
		private void SpawnPlatform()
		{
			if(_lastPlatform == null)
				return;
			Platform newPlatform = Instantiate(_prefabs.GetRandomPlatform(), _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
			_lastPlatform = newPlatform;
		}
	}
}