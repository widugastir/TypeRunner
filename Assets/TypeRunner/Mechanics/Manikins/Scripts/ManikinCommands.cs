using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class ManikinCommands : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Mankin _man;
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		[SerializeField, HideInInspector] private CapsuleCollider _collider;
		[SerializeField, HideInInspector] private AnimationEvents _events;
		[SerializeField, HideInInspector] private StoneThrower _thrower;
		[SerializeField, HideInInspector] private PlayerController _player;
		public bool _isBlocked {get; private set;} = false;
		private Animator _animator;
		private float _baseHeight;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_man = gameObject.GetComponentInChildren<Mankin>();
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
			_collider = gameObject.GetComponentInChildren<CapsuleCollider>();
			_events = gameObject.GetComponentInChildren<AnimationEvents>();
			_thrower = gameObject.GetComponentInChildren<StoneThrower>();
		}
		
		private void Start()
		{
			_animator = _man._animator;
			_player = FindObjectOfType<PlayerController>(true);
			_baseHeight = _collider.height;
		}
		
		public void DoCommand(E_Command command)
		{
			//print(command.ToString() + "____" + _isBlocked);
			if(_isBlocked)
			{
				return;
			}
			
			_animator = _man._animator;
			switch(command)
			{
				case E_Command.Jump:
					Jump();
					break;
				case E_Command.Slide:
					Slide();
					break;
				case E_Command.Swim:
					Swim();
					break;
				case E_Command.Throw:
					Throw();
					break;
				case E_Command.AllThrow:
					_player.SetMove(false);
					_player.SendCommand(E_Command.Throw);
					break;
				case E_Command.Reset:
					Reset();
					break;
				default:
					break;
			}
		}
		
		public void SetBlock(bool blocked)
		{
			_isBlocked = blocked;
		}
		
		private void Reset()
		{
			_player.SetMove(true);
			_collider.height = _baseHeight;
			_man.Immortal = false;
		}
		
		private void Jump()
		{
			_animator.SetTrigger("Jump");
			_man.Movement.Jump(2f, 1f);
		}
		
		private void Slide()
		{
			_collider.height = 0.5f;
			_animator.SetTrigger("Slide");
		}
		
		private void Swim()
		{
			_man.Immortal = true;
			_animator.SetTrigger("Swim");
		}
		
		private void Throw()
		{
			_player.IsMovementEnabled = false;
			_animator.SetTrigger("Throw");
			_events.OnThrow += OnThrow;
			_events.OnAnimationEnd += OnAnimationEnd;
		}
		
		private void OnThrow()
		{
			_events.OnThrow -= OnThrow;
			_thrower.Throw(transform.forward);
		}
		
		private void OnAnimationEnd()
		{
			_events.OnAnimationEnd -= OnAnimationEnd;
			_player.IsMovementEnabled = true;
			_animator.SetTrigger("Running");
			_player.SetMove(true);
		}
		
		public enum E_Command
		{
			None,
			Reset,
			Jump,
			Slide,
			Swim,
			Throw,
			AllThrow
		}
	}
}