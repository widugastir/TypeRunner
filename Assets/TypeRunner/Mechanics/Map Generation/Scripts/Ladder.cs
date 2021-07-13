using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Ladder : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private float _coinsBonusMultiplier = 1f;
		[SerializeField] private float _stepHeight = 1f;
		[SerializeField, HideInInspector] private PlayerController _player;
		public int Rank = 1;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
				_player = FindObjectOfType<PlayerController>(true);
		}
		
		private void Start()
		{
			UpdateReferences(true);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				if(man.Rank == Rank)
				{
					man.EarnedCoinsBonus = _coinsBonusMultiplier;
					_player.ManikinFinished(man);
					//man.Movement.SetCanMove(false);
					//_player.StopSpectateTo(man);
				}
				else
				{
					man.transform.position += Vector3.up * _stepHeight;
				}
			}
		}
	}
}