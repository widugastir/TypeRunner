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
		[SerializeField, HideInInspector] private StoneThrower _thrower;
		[SerializeField, HideInInspector] private PlayerController _player;
		public bool _isBlocked {get; private set;} = false;
		private Animator _animator;
		private float _baseHeight;
		private float _basePosY;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_man = gameObject.GetComponentInChildren<Mankin>();
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
			_collider = gameObject.GetComponentInChildren<CapsuleCollider>();
			_thrower = gameObject.GetComponentInChildren<StoneThrower>();
		}
		
		private void Start()
		{
			_animator = _man._animator;
			_baseHeight = _collider.height;
			_basePosY = _collider.center.y;
		}
		
		public void Init(PlayerController controller)
		{
			_player = controller;
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
				case E_Command.Fly:
					Fly();
					break;
				case E_Command.Climb:
					Climb();
					break;
				case E_Command.Push:
					Push();
					break;
				case E_Command.Double:
					Double();
					break;
				case E_Command.Throw:
					Throw();
					break;
				case E_Command.AllThrow:
					_player.SetMove(false);
					_player.SendCommand(E_Command.Throw, 3);
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
			Vector3 newPos = _collider.center;
			newPos.y = _basePosY;
			_collider.center = newPos;
			_man.SetImmortal(false, 0f, true);
			_animator.SetBool("Fly", false);
			
			//not sure about it
			_player.IsMovementEnabled = true;
			_player._mapMovement.ResetSpeed();
		}
		
		private void Jump()
		{
			_animator.SetTrigger("Jump");
			_man.Movement.Jump(2f, .8f);
		}
		
		private void Double()
		{
			_man.Double();
		}
		
		private void Slide()
		{
			_collider.height = 1.4f;
			Vector3 newPos = _collider.center;
			newPos.y = 0.5f;
			_collider.center = newPos;
			_animator.SetTrigger("Slide");
		}
		
		private void Swim()
		{
			_man.Immortal = true;
			_animator.SetTrigger("Swim");
		}
		
		private void Push()
		{
			_man.Immortal = true;
			_animator.SetTrigger("Push");
		}
		
		private void Fly()
		{
			_animator.SetBool("Fly", true);
			_man.Movement.Flying(OnFlyEnd, OnHoverEnd);
			_player._mapMovement.SetSpeed(12f);
		}
		
		private void Climb()
		{
			//_animator.SetTrigger("Climb");
			_man.Movement.Climb();
			_man.Immortal = true;
			_player._mapMovement.SetSpeed(5f);
		}
		
		private void OnFlyEnd()
		{
			_player.IsMovementEnabled = true;
		}
		
		private void OnHoverEnd()
		{
			_player.IsMovementEnabled = false;
		}
		
		private void Throw()
		{
			//_player.IsMovementEnabled = false;
			_animator.SetTrigger("Throw");
			_man._skinChanger._current._events.OnThrow += OnThrow;
			_man._skinChanger._current._events.OnThrow += OnAnimationEnd;
		}
		
		private void OnThrow()
		{
			_man._skinChanger._current._events.OnThrow -= OnThrow;
			_thrower.Throw(transform.forward);
		}
		
		private void OnAnimationEnd()
		{
			_man._skinChanger._current._events.OnThrow -= OnAnimationEnd;
			_player.IsMovementEnabled = true;
			_player.SetRunningAnimation();
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
			AllThrow,
			Fly,
			Climb,
			Push,
			Double
		}
	}
}