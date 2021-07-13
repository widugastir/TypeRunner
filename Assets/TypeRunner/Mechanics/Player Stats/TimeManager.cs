using UnityEngine;

namespace TypeRunner
{
	public class TimeManager : MonoBehaviour
	{
		private float _fixedDeltaTime;
		
		private void Awake()
	    {
		    _fixedDeltaTime = Time.fixedDeltaTime;
	    }
	    
		private void Update()
		{
			Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
		}
	}
}