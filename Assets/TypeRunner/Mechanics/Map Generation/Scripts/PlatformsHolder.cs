using UnityEngine;

namespace TypeRunner
{
	public class PlatformsHolder : MonoBehaviour
	{
		[SerializeField] private Platform[] _emptyPlatforms;
		[SerializeField] private Platform[] _obstaclePlatforms;
		
		public Platform GetEmptyPlatform()
		{
			int index = Random.Range(0, _emptyPlatforms.Length);
			return _emptyPlatforms[index];
		}
		
		public Platform GetObstaclePlatform()
		{
			int index = Random.Range(0, _obstaclePlatforms.Length);
			return _obstaclePlatforms[index];
		}
	}
}