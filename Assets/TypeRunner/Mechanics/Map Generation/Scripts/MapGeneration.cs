using System.Collections.Generic;
using System.Collections;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MapGeneration : MonoBehaviour, INeedReference
	{
		//------FIELDS
		//[SerializeField] private int _platformAmount = 10;
		//[SerializeField] private int _dailyPlatformAmount = 30;
		//[SerializeField] private int _dailyOneEmptyPerPlatform = 3;
		[SerializeField] public Transform _mapParent;
		[SerializeField] private Platform _basePlatform;
		[SerializeField] private GameObject _ragdollPrefab;
		[SerializeField, HideInInspector] private Platform _lastPlatform;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private PlatformsHolder _prefabs;
		[SerializeField] private LetterPickup LetterPrefab;
		[SerializeField] private Mankin ManikinPrefab;
		private List<GameObject> _ragdolls = new List<GameObject>();
		private List<Platform> _mapPlatforms = new List<Platform>();
		private List<Mankin> _mapManikins = new List<Mankin>();
		private List<LetterPickup> _mapLetters = new List<LetterPickup>();
		public int ManikinsAmount {get {return _mapManikins.Count;}}
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_prefabs = gameObject.GetComponentInChildren<PlatformsHolder>();
				_player = FindObjectOfType<PlayerController>(true);
			}
		}
		
		//private void OnEnable()
		//{
		//	SaveSystem.OnEndLoad += OnEndLoad;
		//}
		
		//private void OnDisable()
		//{
		//	SaveSystem.OnEndLoad -= OnEndLoad;
		//}
		
		//private void OnEndLoad()
		//{
		//	_lastPlatform = _basePlatform;
		//	_player.Init();
		//	StartCoroutine(Generate());
		//}
		
		//private IEnumerator Generate()
		//{
		//	for(int i = 0; i < _platformAmount; i++)
		//	{
		//		SpawnPlatform(i % 2 == 0 ? true : false);
		//		yield return null;
		//	}
		//	SpawnPlatform(_prefabs.GetFinishPlatform());
		//}
		
		//private IEnumerator GenerateDaily()
		//{
		//	for(int i = 0; i < _dailyPlatformAmount; i++)
		//	{
		//		bool isEmptyPlatform = i % _dailyOneEmptyPerPlatform == 0 ? true : false;
		//		SpawnPlatform(isEmptyPlatform, true);
		//		yield return null;
		//	}
		//	SpawnPlatform(_prefabs.GetFinishPlatform());
		//}
		
		//private void SpawnPlatform(Platform prefab)
		//{
		//	if(_lastPlatform == null)
		//		return;
			
		//	Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
		//	newPlatform.Init(this);
		//	_lastPlatform = newPlatform;
		//	_mapPlatforms.Add(newPlatform);
		//	newPlatform.transform.SetParent(_mapParent);
		//}
		
		//private void SpawnPlatform(bool emptyPlatform, bool daily = false)
		//{
		//	if(_lastPlatform == null)
		//		return;
				
		//	Platform prefab = null;
		//	if(emptyPlatform)
		//		prefab = _prefabs.GetEmptyPlatform();
		//	else
		//		prefab = _prefabs.GetObstaclePlatform();
			
		//	Platform newPlatform = Instantiate(prefab, _lastPlatform.ConnectionPoint.position, Quaternion.identity, transform);
		//	newPlatform.Init(this, daily);
		//	_lastPlatform = newPlatform;
		//	_mapPlatforms.Add(newPlatform);
		//	newPlatform.transform.SetParent(_mapParent);
		//}
		
		//public Mankin SpawnManikin(Vector3 position)
		//{
		//	var man = Instantiate(ManikinPrefab, position, Quaternion.identity);
		//	_mapManikins.Add(man);
		//	man.Init(this);
		//	man.transform.SetParent(_mapParent);
		//	return man;
		//}
		
		//public void SpawnRagdoll(Vector3 position)
		//{
		//	var ragdoll = Instantiate(_ragdollPrefab, position, Quaternion.identity, _mapParent);
		//	Destroy(ragdoll, 2f);
		//	_ragdolls.Add(ragdoll);
		//}
		
		//public LetterPickup SpawnLetter(Vector3 position)
		//{
		//	var letter = Instantiate(LetterPrefab, position, Quaternion.identity);
		//	_mapLetters.Add(letter);
		//	letter.transform.SetParent(_mapParent);
		//	return letter;
		//}
		
		//public void DestroyRagdolls()
		//{
		//	foreach(var r in _ragdolls)
		//		if(r != null)
		//			Destroy(r);
		//	_ragdolls.Clear();
		//}
		
		//public void ResetToDaily()
		//{
		//	_player.Init();
		//	DestroyRagdolls();
		//	foreach(var p in _mapPlatforms)
		//		if(p != null)
		//			Destroy(p.gameObject);
		//	_mapPlatforms.Clear();
			
		//	foreach(var m in _mapManikins)
		//		if(m != null)
		//			Destroy(m.gameObject);
		//	_mapManikins.Clear();
			
		//	foreach(var l in _mapLetters)
		//		if(l != null)
		//			Destroy(l.gameObject);
		//	_mapLetters.Clear();
			
		//	_lastPlatform = _basePlatform;
		//	StartCoroutine(GenerateDaily());
		//}
		
		//public void Reset()
		//{
		//	_player.Init();
		//	DestroyRagdolls();
		//	foreach(var p in _mapPlatforms)
		//		if(p != null)
		//			Destroy(p.gameObject);
		//	_mapPlatforms.Clear();
			
		//	foreach(var m in _mapManikins)
		//		if(m != null)
		//			Destroy(m.gameObject);
		//	_mapManikins.Clear();
			
		//	foreach(var l in _mapLetters)
		//		if(l != null)
		//			Destroy(l.gameObject);
		//	_mapLetters.Clear();
			
		//	_lastPlatform = _basePlatform;
		//	StartCoroutine(Generate());
		//}
	}
}