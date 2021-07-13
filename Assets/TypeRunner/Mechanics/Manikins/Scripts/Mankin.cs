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
		public bool Immortal { get; set; } = false;
		public bool IsFinished { get; set; } = false;
		public int RankPosition { get; set; } = 0;
		public int Rank { get; set; } = 0;
		public float EarnedCoinsBonus { get; set; } = 0f;
		
		public static event System.Action<Mankin, bool> OnChangeOwner;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)  
		{
			Movement = gameObject.GetComponentInChildren<ManikinMovement>();
			Commands = gameObject.GetComponentInChildren<ManikinCommands>();
		}
		
		public void SetOwnerTo(bool neutral)
		{
			IsNeutral = neutral;
			OnChangeOwner?.Invoke(this, IsNeutral);
		}
		
		public void SetFinished()
		{
			IsFinished = true;
		}
		
		private void OnCollisionEnter(Collision collisionInfo)
		{
			if(collisionInfo.gameObject.TryGetComponent(out Mankin mankin))
			{
				if(this.IsNeutral == false && mankin.IsNeutral)
					mankin.SetOwnerTo(false);
			}
			
			if(collisionInfo.gameObject.TryGetComponent(out Obstacle obstacle))
			{
				Kill();
			}
		}
		
		public void Kill()
		{
			if(Immortal)
				return;
			SetOwnerTo(true);
			Destroy(gameObject);
		}
	}
}