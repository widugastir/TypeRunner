using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace TypeRunner
{
	public class RainbowOutline : MonoBehaviour
	{
		[SerializeField] private Graphic _image;
		[SerializeField] private Color[] _colors;
		[SerializeField] private float _switchDuration = 0.3f;
		private float _timer = 0;
		private int _currentColor = 0;
		private int _nextColor = 1;
		
		private void OnEnable()
		{
			_timer = 0f;
			_currentColor = 0;
			_nextColor = 1;
		}
		
		private void Update()
		{
			_timer += Time.unscaledDeltaTime;
			if(_timer > _switchDuration)
			{
				_timer = 0f;
				_currentColor++;
				_nextColor++;
				if(_currentColor >= _colors.Length)
					_currentColor = 0;
				if(_nextColor >= _colors.Length)
					_nextColor = 0;
			}
			_image.color = Color.Lerp(_colors[_currentColor], _colors[_nextColor], _timer / _switchDuration);
		}
	}
}