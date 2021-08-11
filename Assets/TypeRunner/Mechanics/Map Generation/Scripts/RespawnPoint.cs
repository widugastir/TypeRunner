using UnityEngine;
using System;

namespace TypeRunner
{
	public class RespawnPoint : MonoBehaviour
	{
		private bool _captured = false;
		public static Action<RespawnPoint> Captured;
		
		private void OnTriggerEnter(Collider other)
		{
			if(_captured)
				return;
				
			if(other.TryGetComponent(out Mankin man))
			{
				Captured?.Invoke(this);
				_captured = true;
			}
		}
	}
}