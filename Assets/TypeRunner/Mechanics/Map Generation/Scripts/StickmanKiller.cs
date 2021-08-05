using UnityEngine;

namespace TypeRunner
{
	public class StickmanKiller : MonoBehaviour
	{
		//------METHODS
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				if(man.Commands._isBlocked)
				{
					man.Kill();
				}
			}
		}
	}
}