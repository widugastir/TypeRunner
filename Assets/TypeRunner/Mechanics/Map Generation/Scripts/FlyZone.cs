using UnityEngine;

namespace TypeRunner
{
	public class FlyZone : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] public float _fullTimer = -1f;
		[SerializeField] public float _bonusTimer = -1f;
		[SerializeField] public float _timeScale = 1f;
		[SerializeField] public E_LetterType[] _requiredWord;
		private bool _isUsed = false;
		public static event System.Action<FlyZone> EnterFlyZone;
		[SerializeField] private float _resetTimer = 0.8f;
		
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
			if(_isUsed == false)
			{
				_isUsed = true;
				Invoke(nameof(ResetUsed), _resetTimer);
				EnterFlyZone?.Invoke(this);
			}
		}
		
		private void ResetUsed()
		{
			_isUsed = false;
		}
	}
}