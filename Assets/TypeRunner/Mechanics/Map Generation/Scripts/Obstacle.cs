using UnityEngine;

namespace TypeRunner
{
	public class Obstacle : MonoBehaviour
	{
		public bool Destructable = false;
		
		private void OnTriggerEnter(Collider other)
		{
			if(Destructable == false)
				return;
				
			if(other.TryGetComponent(out Mankin man))
			{
				Destroy(gameObject);
				
			}
		}
	}
}