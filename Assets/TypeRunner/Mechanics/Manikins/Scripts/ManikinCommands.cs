using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class ManikinCommands : MonoBehaviour
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Mankin _man;
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		private bool _isBlocked = false;
		
		//------METHODS
		[Button]
		private void UpdateReferences()
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
			_isBlocked = true;
		}
		
		private void Jump()
		{
			_man.Movement.Jump(10f);
		}
		
		private void Slide()
		{
			
		}
		
		private void Swim()
		{
			
		}
		
		private void Throw()
		{
			
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