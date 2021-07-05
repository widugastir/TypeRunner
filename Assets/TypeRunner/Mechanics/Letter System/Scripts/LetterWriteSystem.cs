using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class LetterWriteSystem : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private float _timeScale = 0.2f;
		[SerializeField, HideInInspector] private LettersPanel _lettersPanel;
		
		//------METHODS
		[Button]
		private void UpdateReferences()
		{
			_lettersPanel = gameObject.GetComponentInChildren<LettersPanel>();
		}
		
		private void OnEnable()
		{
			ObstacleZone.OnPlayerEntered += PlayerEnterZone;
			ObstacleZone.OnPlayerExit += PlayerExitZone;
		}
		
		private void OnDisable()
		{
			ObstacleZone.OnPlayerEntered -= PlayerEnterZone;
			ObstacleZone.OnPlayerExit -= PlayerExitZone;
		}
		
		private void PlayerEnterZone(ObstacleZone zone, E_LetterType[] word)
		{
			EnableWordWritter(zone, word);
		}
		
		private void PlayerExitZone(ObstacleZone zone)
		{
			DisableWordWritter(zone);
		}
		
		private void EnableWordWritter(ObstacleZone zone, E_LetterType[] word)
		{
			Time.timeScale = _timeScale;
			_lettersPanel.Activate();
		}
		
		private void DisableWordWritter(ObstacleZone zone)
		{
			Time.timeScale = 1f;
			_lettersPanel.Disable();
		}
	}
}