using UnityEngine;

namespace TypeRunner
{
	public class Obstacle : MonoBehaviour
	{
		public bool Destructable = false;
		[SerializeField] private Collider _collider;
		public ObstacleChild[] Childs;
		public GameObject Destructed;
		
		private void OnTriggerEnter(Collider other)
		{
			if(Destructable == false)
				return;
				
			if(other.TryGetComponent(out Mankin man))
			{
				Destroy(gameObject);
			}
		}
		
		protected void OnCollisionEnter(Collision collisionInfo)
		{
			if(Destructable == false)
				return;
				
			if(collisionInfo.gameObject.TryGetComponent(out Mankin man))
			{
				if(Destructed != null)
				{
					var d = Instantiate(Destructed, transform.position, transform.rotation);
					if(transform.parent != null)
					{
						d.transform.SetParent(transform.parent);
					}
					Destroy(d, 2f);
				}
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