using UnityEngine;

namespace TypeRunner
{
	public class ColorChanger : MonoBehaviour
	{
		[SerializeField] private Color _baseColor;
		public Color PlayerColor { get { return _baseColor;} set { _baseColor = value;} }
		
		public static event System.Action<Color> OnColorChange;
		
		public void SetPlayerColor(Color color)
		{
			PlayerColor = color;
			OnColorChange?.Invoke(PlayerColor);
		}
	}
}