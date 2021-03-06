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
		[SerializeField] private Ragdoll _ragdollPrefab;
		[SerializeField] private LetterPickup LetterPrefab;
		[SerializeField] private Mankin ManikinPrefab;
		[SerializeField] private Level[] _levels;
		[SerializeField] private Level[] _tutorialLevels;
		[SerializeField] private DailyGenerator _dailyGenerator;
		[SerializeField] private PlayerStats _stats;
		[HideInInspector] public Transform _levelParent;
		[SerializeField] private CoinManager _coinManager;
		private Level _level;
		
		[SerializeField, HideInInspector] public PlayerController _player;
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
				_stats = FindObjectOfType<PlayerStats>(true);
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
			ClearMap();
			if(_stats._currentLevelIndex >= _levels.Length)
				_stats._currentLevelIndex = 0;
			
			if(_stats._currentLevel < _tutorialLevels.Length + 1)
			{
				_level = Instantiate(_tutorialLevels[_stats._currentLevel - 1], _firstPlatform.ConnectionPoint.position, Quaternion.identity);
			}
			else
			{
				int index = _stats._currentLevelIndex;
				_level = Instantiate(_levels[index], _firstPlatform.ConnectionPoint.position, Quaternion.identity);
				_stats._currentLevelIndex++;
			}
			_level.Init(this, _coinManager);
			_level.transform.SetParent(_levelParent);
			
		}
		
		public void ClearMap()
		{
			if(_level != null)
			{
				Destroy(_level.gameObject);
			}
			
			if(_levelParent != null)
				Destroy(_levelParent.gameObject);
			_levelParent = new GameObject().transform;
			_levelParent.SetParent(_map);
		}
		
		public Mankin SpawnManikin(Vector3 position)
		{
			if(_player._manikins.Count >= _stats.MaxStickmans)
				return null;
			var man = Instantiate(ManikinPrefab, position, Quaternion.identity);
			_mapManikins.Add(man);
			man.Init(this);
			man.transform.SetParent(_levelParent);
			return man;
		}
		
		public LetterPickup SpawnLetter(Vector3 position)
		{
			var letter = Instantiate(LetterPrefab, position, Quaternion.identity);
			_mapLetters.Add(letter);
			letter.transform.SetParent(_levelParent);
			return letter;
		}
		
		public void SpawnRagdoll(Vector3 position, Vector3 force)
		{
			var ragdoll = Instantiate(_ragdollPrefab, position, Quaternion.identity);
			ragdoll.transform.SetParent(_levelParent);
			//if(force.magnitude > 0f)
			//{
			//	ragdoll.AddForce(force, ForceMode.VelocityChange);
			//}
			Destroy(ragdoll, 7f);
			_ragdolls.Add(ragdoll.gameObject);
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
			ClearMap();
			Reset();
			_dailyGenerator.StartGenerate(this);
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