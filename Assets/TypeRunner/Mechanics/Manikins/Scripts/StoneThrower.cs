using UnityEngine;

namespace TypeRunner
{
	public class StoneThrower : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPos;
		[SerializeField] private Projectile _prefab;
		[SerializeField] private float _speed = 1;
		
		public void Throw(Vector3 direction)
		{
			Projectile stone = Instantiate(_prefab, _spawnPos.position, Quaternion.identity);
			stone.Init(_speed, direction);
		}
	}
}