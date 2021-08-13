using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class ImmortalTimer : MonoBehaviour
	{
		[SerializeField] private Image _fade;
		private IEnumerator _loop;
		private float _duration;
		private float _currentDuration;
		
		protected void OnDisable() => Disable();
		
		public void Enable(float duration)
		{
			gameObject.SetActive(true);
			_currentDuration = 0f;
			_duration = duration;
			_loop = TimerLoop();
			StartCoroutine(_loop);
		}
		
		public void Disable()
		{
			_currentDuration = 0f;
			_duration = 0f;
			if(_loop != null)
			{
				StopCoroutine(_loop);
			}
			gameObject.SetActive(false);
		}
		
		private IEnumerator TimerLoop()
		{
			while(true)
			{
				yield return null;
				_currentDuration += Time.unscaledDeltaTime;
				_fade.fillAmount = _currentDuration / _duration;
				if(_currentDuration >= _duration)
				{
					Disable();
					break;
				}
			}
		}
		
		public void Reset()
		{
			Disable();
		}
	}
}