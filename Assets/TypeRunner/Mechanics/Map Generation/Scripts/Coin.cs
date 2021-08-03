using UnityEngine;

namespace TypeRunner
{
	public class Coin : MonoBehaviour
	{
		[SerializeField] private int _amount = 1;
		[SerializeField] private CoinManager _coinManager;
		
		public void Init(CoinManager coinManager)
		{
			_coinManager = coinManager;
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
			_coinManager.AddEarnedCoins(_amount);
			Destroy(gameObject);
		}
	}
}