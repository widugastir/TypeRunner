using UnityEngine;

namespace TypeRunner
{
	public class PlatformsHolder : MonoBehaviour
	{
		[SerializeField] private Platform[] _platforms;
		
		public Platform GetRandomPlatform()
		{
			int index = Random.Range(0, _platforms.Length);
			return _platforms[index];
		}
	}
}