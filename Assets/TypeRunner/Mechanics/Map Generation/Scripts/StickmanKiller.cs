using UnityEngine;

namespace TypeRunner
{
	public class StickmanKiller : MonoBehaviour
	{
		[SerializeField] private bool _killOnlyBlocked = true;
		
		//------METHODS
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				if(_killOnlyBlocked && man.Commands._isBlocked || _killOnlyBlocked == false)
				{
					man.Kill();
				}
			}
		}
	}
}