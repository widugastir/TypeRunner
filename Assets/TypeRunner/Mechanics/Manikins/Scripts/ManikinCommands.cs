using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class ManikinCommands : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Mankin _man;
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		private bool _isBlocked = false;
		
		//------METHODS
		public void UpdateReferences()
		{
			_man = gameObject.GetComponentInChildren<Mankin>();
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
		}
		
		public void DoCommand(E_Command command)
		{
			if(_isBlocked)
				return;
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
				default:
					break;
			}
		}
		
		public void BlockCommand()
		{
			print("BlockCommand");
			_isBlocked = true;
		}
		
		private void Jump()
		{
			_man.Movement.Jump(10f);
			print("Jump");
		}
		
		private void Slide()
		{
			print("Slide");
			
		}
		
		private void Swim()
		{
			print("Swim");
			
		}
		
		private void Throw()
		{
			print("Throw");
			
		}
		
		public enum E_Command
		{
			None,
			Jump,
			Slide,
			Swim,
			Throw
		}
	}
}