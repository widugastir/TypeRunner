using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class DailyGenerator : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private int _dailyPlatformAmount = 30;
		[SerializeField] private int _dailyOneEmptyPerPlatform = 3;
		[SerializeField] private Platform _basePlatform;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField] private CoinManager _coinManager;
		[SerializeField] private PlatformsHolder _prefabs;
		[SerializeField] private LetterPickup LetterPrefab;
		[SerializeField] private Mankin ManikinPrefab;
		private Platform _lastPlatform;
		private MapGenerationLevels _baseGenerator;
		private List<GameObject> _ragdolls = new List<GameObject>();
		private List<Platform> _mapPlatforms = new List<Platform>();
		private List<Mankin> _mapManikins = new List<Mankin>();
		private List<LetterPickup> _mapLetters = new List<LetterPickup>();
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_player = FindObjectOfType<PlayerController>(true);
			}
		}
		
		public void StartGenerate(MapGenerationLevels baseGenerator)
		{
			_baseGenerator = baseGenerator;
			StartCoroutine(GenerateDaily());
		}
		
		private IEnumerator GenerateDaily()
		{
			_lastPlatform = _basePlatform;
			_baseGenerator.ClearMap();
			for(int i = 0; i < _dailyPlatformAmount; i++)
			{
				bool isEmptyPlatform = i % _dailyOneEmptyPerPlatform == 0 ? true : false;
				SpawnPlatform(isEmptyPlatform, true);
				yield return null;
			}
			SpawnPlatform(_prefabs.GetFinishPlatform());
		}
		
		private void SpawnPlatform(Platform prefab)
		{
			if(_lastPlatform == null)
				return;
			
			Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, _baseGenerator._levelParent);
			newPlatform.Init(_baseGenerator, _baseGenerator._levelParent, _coinManager);
			_lastPlatform = newPlatform;
			_mapPlatforms.Add(newPlatform);
		}
		
		private void SpawnPlatform(bool emptyPlatform, bool daily = false)
		{
			if(_lastPlatform == null)
				return;
				
			Platform prefab = null;
			if(emptyPlatform)
			{
				//print(_prefabs);
				//print(_prefabs.GetEmptyPlatform());
				prefab = _prefabs.GetEmptyPlatform();
			}
			else
				prefab = _prefabs.GetObstaclePlatform();
			
			Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity);
			newPlatform.Init(_baseGenerator, _baseGenerator._levelParent, _coinManager, daily);
			_lastPlatform = newPlatform;
			_mapPlatforms.Add(newPlatform);
			newPlatform.transform.SetParent(_baseGenerator._levelParent);
		}
	}
}