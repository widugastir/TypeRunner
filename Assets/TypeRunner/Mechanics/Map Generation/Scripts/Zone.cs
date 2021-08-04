using UnityEngine;

namespace TypeRunner
{
	public class Zone : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private ManikinCommands.E_Command _command;
		[SerializeField] private bool _useIfSuccessful = true;
		[SerializeField] private bool _callExitEvent = true;
		private bool _isUsed = false;
		public static event System.Action EnterZone;
		
		//------METHODS
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				ManEnter(man);
			}
		}
		
		private void ManEnter(Mankin man)
		{
			if(_useIfSuccessful && man.IsWordSuccessful == true)
			{
				man.Commands.DoCommand(_command);
			}
			else if(_useIfSuccessful == false)
			{
				man.Commands.DoCommand(_command);
			}
			
			if(_isUsed == false && _callExitEvent)
			{
				_isUsed = true;
				EnterZone?.Invoke();
			}
		}
	}
}