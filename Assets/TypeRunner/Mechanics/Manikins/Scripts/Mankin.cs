using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Mankin : MonoBehaviour, INeedReference
	{
		//------FIELDS
		public bool IsNeutral = true;
		[HideInInspector] public ManikinMovement Movement;
		[HideInInspector] public ManikinCommands Commands;
		
		public static event System.Action<Mankin, bool> OnChangeOwner;
		
		//------METHODS
		public void UpdateReferences()
		{
			Movement = gameObject.GetComponentInChildren<ManikinMovement>();
			Commands = gameObject.GetComponentInChildren<ManikinCommands>();
		}
		
		public void SetOwnerToPlayer()
		{
			IsNeutral = false;
			OnChangeOwner?.Invoke(this, IsNeutral);
		}
		
		public void SetOwnerToNeutral()
		{
			IsNeutral = true;
			OnChangeOwner?.Invoke(this, IsNeutral);
		}
		
		private void OnCollisionEnter(Collision collisionInfo)
		{
			if(collisionInfo.gameObject.TryGetComponent(out Mankin mankin))
			{
				if(this.IsNeutral == false && mankin.IsNeutral)
					mankin.SetOwnerToPlayer();
			}
			
			if(collisionInfo.gameObject.TryGetComponent(out Obstacle obstacle))
			{
				Destroy(gameObject);
			}
		}
		
		private void OnDestroy()
		{
			SetOwnerToNeutral();
		}
	}
}