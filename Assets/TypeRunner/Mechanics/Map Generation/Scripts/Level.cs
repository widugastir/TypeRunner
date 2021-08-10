using UnityEngine;

namespace TypeRunner
{
	public class Level : MonoBehaviour
	{
		private MapGenerationLevels _generator;
		[SerializeField] private Platform[] _platforms;
		
		public void Init(MapGenerationLevels generator)
		{
			_generator = generator;
			InitPlatforms();
		}
		
		private void InitPlatforms()
		{
			foreach(var p in _platforms)
			{
				p.Init(_generator, transform, false);
			}
		}
	}
}