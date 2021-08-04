using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine;

namespace TypeRunner
{
	public class ObstacleZone : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] public float _fullTimer = -1f;
		[SerializeField] public float _bonusTimer = -1f;
		[SerializeField] public float _timeScale = 1f;
		[SerializeField] public int _manikinsToBlock = 1;
		[SerializeField] private bool _oneCommandOnly = false;
		[SerializeField] private bool _isEnterZone = true;
		[SerializeField] private bool _onlySuccessful = false;
		[SerializeField] public bool _disableStrafe = false;
		[SerializeField] private ManikinCommands.E_Command _command;
		[ShowIf("_isEnterZone"), SerializeField] private E_LetterType[] _requiredLettersWrite;
		private bool _isUsed = false;
		
		public UnityEvent _onSuccessful;
		
		public static event System.Action<ObstacleZone, E_LetterType[]> OnPlayerEntered;
		public static event System.Action<ObstacleZone> EnterZone;
		public static event System.Action<ObstacleZone> OnPlayerExit;
		
		//------METHODS
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				
				EnterZone?.Invoke(this);
				if(man.Commands._isBlocked)
					return;
					
				bool successful = (_onlySuccessful && man.IsWordSuccessful == true || _onlySuccessful == false);
				
				if(_isUsed == false && man.AffectZones == true)
				{
					_isUsed = true;
					if(_isEnterZone)
						OnPlayerEntered?.Invoke(this, _requiredLettersWrite);
					else if(_isEnterZone == false)
						OnPlayerExit?.Invoke(this);
					
					successful = (_onlySuccessful && man.IsWordSuccessful == true || _onlySuccessful == false);
					if(_oneCommandOnly && successful)
					{
						man.Commands.DoCommand(_command);
					}
				}
				successful = (_onlySuccessful && man.IsWordSuccessful == true || _onlySuccessful == false);
				if(_oneCommandOnly == false && successful)
				{
					man.Commands.DoCommand(_command);
				}
				
				if(successful)
				{
					_onSuccessful?.Invoke();
				}
			}
		}
	}
}