using UnityEngine;

namespace TypeRunner
{
	public class Obstacle : MonoBehaviour
	{
		public bool Destructable = false;
		[SerializeField] private Collider _collider;
		public ObstacleChild[] Childs;
		public GameObject DestroyThis;
		public GameObject Destructed;
		private bool _isDestructed = false;
		
		private void OnTriggerEnter(Collider other)
		{
			if(Destructable == false)
				return;
				
			if(other.TryGetComponent(out Mankin man))
			{
				if(DestroyThis != null)
					Destroy(DestroyThis);
				else
					Destroy(gameObject);
			}
		}
		
		protected void OnCollisionEnter(Collision collisionInfo)
		{
			if(Destructable == false || _isDestructed == true)
				return;
			_isDestructed = true;
			if(collisionInfo.gameObject.TryGetComponent(out Mankin man))
			{
				if(Destructed != null)
				{
					var d = Instantiate(Destructed, transform.position, transform.rotation);
					if(DestroyThis != null)
					{
						d.transform.position = DestroyThis.transform.position;
						d.transform.rotation = DestroyThis.transform.rotation;
					}
					if(DestroyThis != null && DestroyThis.transform.parent != null)
						d.transform.SetParent(DestroyThis.transform.parent);
					else if(transform.parent != null)
					{
						d.transform.SetParent(transform.parent);
					}
					Destroy(d, 2f);
				}
				if(DestroyThis != null)
					Destroy(DestroyThis);
				else
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
					if(c != null)
						c.Activate(forcePoint);
				}
			}
			if(DestroyThis != null)
				Destroy(DestroyThis);
			else
				Destroy(gameObject);
		}
	}
}