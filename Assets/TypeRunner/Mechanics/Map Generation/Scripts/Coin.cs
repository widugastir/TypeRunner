using UnityEngine;

namespace TypeRunner
{
	public class Coin : MonoBehaviour
	{
		[SerializeField] private int _amount = 1;
		[SerializeField] private ParticleSystem _pickupVFX;
		private MapGenerationLevels _map;
		private CoinManager _coinManager;
		
		public void Init(CoinManager coinManager, MapGenerationLevels map)
		{
			_coinManager = coinManager;
			_map = map;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Mankin man))
			{
				Kill();
			}
		}
		
		private void Kill()
		{
			_pickupVFX.transform.SetParent(_map._levelParent);
			_pickupVFX.Play();
			_coinManager.AddEarnedCoins(_amount);
			Destroy(gameObject);
		}
	}
}