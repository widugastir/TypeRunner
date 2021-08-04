using UnityEngine;

namespace TypeRunner
{
	public class FlyZone : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] public E_LetterType[] _requiredWord;
		private bool _isUsed = false;
		public static event System.Action<FlyZone> EnterFlyZone;
		
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
				EnterFlyZone?.Invoke(this);
			}
		}
	}
}