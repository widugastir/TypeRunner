using UnityEngine;

namespace TypeRunner
{
	public class Level : MonoBehaviour
	{
		private MapGenerationLevels _generator;
		[SerializeField] private Platform[] _platforms;
		
		public void Init(MapGenerationLevels generator, CoinManager coinManager)
		{
			_generator = generator;
			InitPlatforms(coinManager);
		}
		
		private void InitPlatforms(CoinManager coinManager)
		{
			foreach(var p in _platforms)
			{
				p.Init(_generator, transform, coinManager, false);
			}
		}
	}
}