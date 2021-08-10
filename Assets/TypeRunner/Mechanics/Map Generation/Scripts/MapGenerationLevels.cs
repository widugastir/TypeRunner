using System.Collections.Generic;
using System.Collections;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MapGenerationLevels : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private Transform _map;
		[SerializeField] private Platform _firstPlatform;
		[SerializeField] private GameObject _ragdollPrefab;
		[SerializeField] private LetterPickup LetterPrefab;
		[SerializeField] private Mankin ManikinPrefab;
		[SerializeField] private Level[] _levels;
		private Level _level;
		
		[SerializeField, HideInInspector] private PlayerController _player;
		private List<GameObject> _ragdolls = new List<GameObject>();
		private List<Mankin> _mapManikins = new List<Mankin>();
		private List<LetterPickup> _mapLetters = new List<LetterPickup>();
		public int ManikinsAmount {get {return _mapManikins.Count;}}
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_player = FindObjectOfType<PlayerController>(true);
			}
		}
		
		private void OnEnable()
		{
			SaveSystem.OnEndLoad += OnEndLoad;
		}
		
		private void OnDisable()
		{
			SaveSystem.OnEndLoad -= OnEndLoad;
		}
		
		private void OnEndLoad()
		{
			_player.Init();
			Generate();
		}
		
		public void Generate()
		{
			int index = Random.Range(0, _levels.Length);
			if(_level != null)
			{
				Destroy(_level.gameObject);
			}
			_level = Instantiate(_levels[index], _firstPlatform.ConnectionPoint.position, Quaternion.identity);
			_level.Init(this);
			_level.transform.SetParent(_map);
		}
		
		public Mankin SpawnManikin(Vector3 position)
		{
			var man = Instantiate(ManikinPrefab, position, Quaternion.identity);
			_mapManikins.Add(man);
			man.Init(this);
			man.transform.SetParent(_map);
			return man;
		}
		
		public LetterPickup SpawnLetter(Vector3 position)
		{
			var letter = Instantiate(LetterPrefab, position, Quaternion.identity);
			_mapLetters.Add(letter);
			letter.transform.SetParent(_level.transform);
			return letter;
		}
		
		public void SpawnRagdoll(Vector3 position)
		{
			var ragdoll = Instantiate(_ragdollPrefab, position, Quaternion.identity, _level.transform);
			Destroy(ragdoll, 2f);
			_ragdolls.Add(ragdoll);
		}
		
		public void DestroyRagdolls()
		{
			foreach(var r in _ragdolls)
				if(r != null)
					Destroy(r);
			_ragdolls.Clear();
		}
		
		public void ResetToDaily()
		{
			Reset();
		}
		
		public void Reset()
		{
			_player.Init();
			DestroyRagdolls();
			for(int i = 0; i < _mapManikins.Count; i++)
			{
				if(_mapManikins[i] != null)
					Destroy(_mapManikins[i].gameObject);
			}
			_mapManikins.Clear();
		}
	}
}