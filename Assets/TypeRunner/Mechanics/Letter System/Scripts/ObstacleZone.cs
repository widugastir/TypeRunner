using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class ObstacleZone : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private bool _oneCommandOnly = false;
		[SerializeField] private bool _isEnterZone = true;
		[SerializeField] private ManikinCommands.E_Command _command;
		[ShowIf("_isEnterZone"), SerializeField] private E_LetterType[] _requiredLettersWrite;
		private bool _isUsed = false;
		
		public static event System.Action<ObstacleZone, E_LetterType[]> OnPlayerEntered;
		public static event System.Action<ObstacleZone> OnPlayerExit;
		
		//------METHODS
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				if(man.Commands._isBlocked)
					return;
				if(_isUsed == false)
				{
					_isUsed = true;
					if(_isEnterZone)
						OnPlayerEntered?.Invoke(this, _requiredLettersWrite);
					else if(_isEnterZone == false)
						OnPlayerExit?.Invoke(this);
					if(_oneCommandOnly)
						man.Commands.DoCommand(_command);
				}
				if(_oneCommandOnly == false)
				{
					man.Commands.DoCommand(_command);
				}
			}
		}
	}
}