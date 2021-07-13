using UnityEngine;

namespace TypeRunner
{
	public class CameraResetter : MonoBehaviour
	{
		private Vector3 _basePosition;
		
		private void Start()
		{
			_basePosition = transform.position;
		}
		
		public void Reset()
		{
			transform.position = _basePosition;
		}
	}
}