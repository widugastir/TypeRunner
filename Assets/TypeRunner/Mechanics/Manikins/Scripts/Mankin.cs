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
		private bool IsIdle { get; set; } = false;
		public bool Immortal { get; set; } = false;
		public bool IsFinished { get; set; } = false;
		public int RankPosition { get; set; } = 0;
		public int Rank { get; set; } = 0;
		public float EarnedCoinsBonus { get; set; } = 0f;
		
		public static event System.Action<Mankin, bool> OnChangeOwner;
		[SerializeField, HideInInspector] public Animator _animator;
		[SerializeField, HideInInspector] public SkinChanger _skinChanger;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)  
		{
			if(sceneObject == false)
			{
				Movement = gameObject.GetComponentInChildren<ManikinMovement>();
				Commands = gameObject.GetComponentInChildren<ManikinCommands>();
				_animator = gameObject.GetComponentInChildren<Animator>();
				_skinChanger = gameObject.GetComponentInChildren<SkinChanger>();
			}
		}
		
		private void OnEnable()
		{
			Shop.OnSkinSelect += OnSkinSelect;
		}
		
		private void OnDisable()
		{
			Shop.OnSkinSelect -= OnSkinSelect;
		}
		
		private void OnSkinSelect()
		{
			_animator = _skinChanger._current.animator;
		}
		
		public void SetOwnerTo(bool neutral)
		{
			IsNeutral = neutral;
			OnChangeOwner?.Invoke(this, IsNeutral);
			_animator = _skinChanger._current.animator;
			if(neutral == false && IsIdle == false)
			{
				_animator.SetTrigger("Running");
			}
		}
		
		public void SetIdle(bool idle)
		{
			_animator = _skinChanger._current.animator;
			IsIdle = idle;
			if(IsIdle)
			{
				//_animator.SetTrigger("Idle");
			}
			else
			{
				_animator.SetTrigger("Running");
			}
		}
		
		public void SetFinished()
		{
			_animator.SetTrigger("Dance");
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
		
		public void BlockStickman(string layer, bool moveForward = false)
		{
			gameObject.layer = LayerMask.NameToLayer(layer);
			Movement.MoveToGroupCenter = false;
			
			if(moveForward)
			{
				Movement.SetCanMove2(true);
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