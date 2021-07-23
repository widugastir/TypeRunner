using UnityEngine;

namespace TypeRunner
{
	public class Obstacle : MonoBehaviour
	{
		public bool Destructable = false;
		[SerializeField] private Collider _collider;
		public ObstacleChild[] Childs;
		
		private void OnTriggerEnter(Collider other)
		{
			if(Destructable == false)
				return;
				
			if(other.TryGetComponent(out Mankin man))
			{
				Destroy(gameObject);
			}
		}
		
		public void Kill(Vector3 forcePoint)
		{
			_collider.enabled = false;
			
			if(Childs.Length > 0)
			{
				foreach(var c in Childs)
				{
					c.Activate(forcePoint);
				}
			}
			Destroy(gameObject);
		}
	}
}