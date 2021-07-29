using UnityEngine;

namespace TypeRunner
{
	public class StoneThrower : MonoBehaviour
	{
		[SerializeField] private SkinChanger _skinChanger;
		[SerializeField] private Projectile _prefab;
		[SerializeField] private float _speed = 1;
		
		public void Throw(Vector3 direction)
		{
			print(1);
			Vector3 spawnPosition = _skinChanger._current._hand.position;
			Projectile stone = Instantiate(_prefab, spawnPosition, Quaternion.identity);
			stone.Init(_speed, direction);
		}
	}
}