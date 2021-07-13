using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Projectile : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		[SerializeField] private float _lifetime = 2f;
		private float _speed;
		private Vector3 _direction;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)  
		{
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
		}
		
		public void Init(float speed, Vector3 direction)
		{
			_direction = direction;
			_speed = speed;
			Destroy(gameObject, _lifetime);
		}
		
		private void FixedUpdate()
		{
			Move();
		}
		
		private void Move()
		{
			_rigi.MovePosition(transform.position + _direction * _speed * Time.fixedDeltaTime);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man) || other.isTrigger)
				return;
			
			if(other.TryGetComponent(out Obstacle obstacle))
			{
				if(obstacle.Destructable)
				{
					Destroy(obstacle.gameObject);
				}
			}
			
			Destroy(gameObject);
		}
	}
}