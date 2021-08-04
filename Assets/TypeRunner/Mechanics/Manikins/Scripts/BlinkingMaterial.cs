using UnityEngine;

namespace TypeRunner
{
	public class BlinkingMaterial : MonoBehaviour
	{
		[SerializeField] private Renderer[] _renderers;
		[SerializeField] private bool _isActive = false;
		[SerializeField] private float _delay = 0.1f;
		private bool _invisible = false;
		private float _timer = 0f;
		private float _baseDelay = 0f;
		private float _duration = 0f;
		private float _fullTimer = 0f;
		private float _startDelay = 0f;
		private float _endDelay = 0f;
		
		private void Start()
		{
			_baseDelay = _delay;
		}
 
		private void Update()
		{
			if(_isActive == false)
				return;
			
			if (_timer >= _delay)
			{
				_invisible = !_invisible;
				for(int i = 0; i < _renderers.Length; i++)
				{
					UpdateMaterial(_renderers[i]);
				}
				_timer = 0f;
			}
			if(_duration > 0f && _startDelay > 0f && _endDelay > 0f)
			{
				_delay = Mathf.Lerp(_startDelay, _endDelay, _fullTimer / _duration);
			}
			
			_timer += Time.unscaledDeltaTime;
			_fullTimer += Time.unscaledDeltaTime;
		}
		
		public void SetDuration(float duration, float startFlickDelay = -1f, float endFlickDelay = -1f)
		{
			_duration = duration;
			if(startFlickDelay > 0f)
			{
				_startDelay = startFlickDelay;
				_delay = startFlickDelay;
			}
			if(endFlickDelay > 0f)
			{
				_endDelay = endFlickDelay;
			}
		}
		
		public void Enable(bool reset = true)
		{
			if(reset)
			{
				ResetDelay();
			}
			_isActive = true;
		}
		
		public void Disable()
		{
			_isActive = false;
			_fullTimer = 0f;
			_duration = 0f;
			EnableAll();
		}
		
		public void SetDelay(float delay)
		{
			_delay = delay;
		}
		
		public void ResetDelay()
		{
			_delay = _baseDelay;
		}
		
		private void EnableAll()
		{
			for(int i = 0; i < _renderers.Length; i++)
			{
				_renderers[i].enabled = true;
			}
		}
		
		private void UpdateMaterial(Renderer renderer)
		{
			if (_invisible)
				renderer.enabled = false;
			else
				renderer.enabled = true;
		}
	}
}