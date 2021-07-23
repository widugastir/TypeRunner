using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Finish : MonoBehaviour, INeedReference
	{
		//------FIELDS
		private bool _isUsed = false;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_player = FindObjectOfType<PlayerController>(true);
				_levelManager = FindObjectOfType<LevelManager>(true);
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
					_player.SetIndependentMove(true);
					_player.MakeFormationLine();
				}
			}
		}
	}
}