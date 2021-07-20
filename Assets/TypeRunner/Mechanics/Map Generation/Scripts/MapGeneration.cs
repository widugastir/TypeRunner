using System.Collections.Generic;
using System.Collections;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MapGeneration : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _platformAmount = 10;
		[SerializeField] private int _dailyPlatformAmount = 30;
		[SerializeField] private int _dailyOneEmptyPerObstacle = 3;
		[SerializeField] private Platform _basePlatform;
		[SerializeField, HideInInspector] private Platform _lastPlatform;
		
		[SerializeField, HideInInspector] private PlatformsHolder _prefabs;
		[SerializeField] private LetterPickup LetterPrefab;
		[SerializeField] private Mankin ManikinPrefab;
		private List<Platform> _mapPlatforms = new List<Platform>();
		private List<Mankin> _mapManikins = new List<Mankin>();
		private List<LetterPickup> _mapLetters = new List<LetterPickup>();
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_prefabs = gameObject.GetComponentInChildren<PlatformsHolder>();
		}
		
		private void Start()
		{
			_lastPlatform = _basePlatform;
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
		
		private IEnumerator GenerateDaily()
		{
			for(int i = 0; i < _dailyPlatformAmount; i++)
			{
				bool isEmptyPlatform = i % _dailyOneEmptyPerObstacle == 0 ? true : false;
				SpawnPlatform(isEmptyPlatform, true);
				yield return null;
			}
			SpawnPlatform(_prefabs.GetFinishPlatform());
		}
		
		private void SpawnPlatform(Platform prefab)
		{
			if(_lastPlatform == null)
				return;
			
			Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
			newPlatform.Init(this);
			_lastPlatform = newPlatform;
			_mapPlatforms.Add(newPlatform);
		}
		
		private void SpawnPlatform(bool emptyPlatform, bool daily = false)
		{
			if(_lastPlatform == null)
				return;
				
			Platform prefab = null;
			if(emptyPlatform)
				prefab = _prefabs.GetEmptyPlatform();
			else
				prefab = _prefabs.GetObstaclePlatform();
			
			Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
			newPlatform.Init(this, daily);
			_lastPlatform = newPlatform;
			_mapPlatforms.Add(newPlatform);
		}
		
		public Mankin SpawnManikin(Vector3 position)
		{
			var man = Instantiate(ManikinPrefab, position, Quaternion.identity);
			_mapManikins.Add(man);
			return man;
		}
		
		public LetterPickup SpawnLetter(Vector3 position)
		{
			var letter = Instantiate(LetterPrefab, position, Quaternion.identity);
			_mapLetters.Add(letter);
			return letter;
		}
		
		public void ResetToDaily()
		{
			foreach(var p in _mapPlatforms)
				if(p != null)
					Destroy(p.gameObject);
			_mapPlatforms.Clear();
			
			foreach(var m in _mapManikins)
				if(m != null)
					Destroy(m.gameObject);
			_mapManikins.Clear();
			
			foreach(var l in _mapLetters)
				if(l != null)
					Destroy(l.gameObject);
			_mapLetters.Clear();
			
			_lastPlatform = _basePlatform;
			StartCoroutine(GenerateDaily());
		}
		
		public void Reset()
		{
			foreach(var p in _mapPlatforms)
				if(p != null)
					Destroy(p.gameObject);
			_mapPlatforms.Clear();
			
			foreach(var m in _mapManikins)
				if(m != null)
					Destroy(m.gameObject);
			_mapManikins.Clear();
			
			foreach(var l in _mapLetters)
				if(l != null)
					Destroy(l.gameObject);
			_mapLetters.Clear();
			
			_lastPlatform = _basePlatform;
			StartCoroutine(Generate());
		}
	}
}