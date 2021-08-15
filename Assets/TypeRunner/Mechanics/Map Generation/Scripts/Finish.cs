using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Finish : MonoBehaviour, INeedReference
	{
		//------FIELDS
		private bool _isUsed = false;
		[SerializeField, HideInInspector] private CinematicSystem _cinematic;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private LetterWriteSystem _letterWriteSystem;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_cinematic = FindObjectOfType<CinematicSystem>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_player = FindObjectOfType<PlayerController>(true);
				_levelManager = FindObjectOfType<LevelManager>(true);
				_letterWriteSystem = FindObjectOfType<LetterWriteSystem>(true);
			}
		}
		
		private void Start()
		{
			UpdateReferences(true);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				if(_isUsed == false)
				{
					_isUsed = true;
					_cinematic.PlayMultiplyView(OnMultiplyAnim);
				}
			}
		}
		
		private void OnMultiplyAnim()
		{
			_letterWriteSystem.DisableAfterFinish();
			_player.MultiplyStikmans(_stats.SuccessfulMultiplier);
			_player.SetIndependentMove(true);
			_player.MakeFormationTriangle();
		}
	}
}