using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class LevelChunk : MonoBehaviour
	{
		[SerializeField] private Color _highlightColor;
		[SerializeField] private Color _baseColor;
		[SerializeField] private Image _image;
		
		public void Highlight()
		{
			_image.color = _highlightColor;
		}
		
		public void Downlight()
		{
			_image.color = _baseColor;
		}
	}
}