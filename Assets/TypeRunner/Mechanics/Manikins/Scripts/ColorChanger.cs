using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class ColorChanger : MonoBehaviour, INeedReference
	{
		[SerializeField] private Color _baseColor;
		public Color PlayerColor { get { return _baseColor;} set { _baseColor = value;} }
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		public static event System.Action<Color> OnColorChange;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
			}
		}
		
		private void OnEnable()
		{
			SaveSystem.OnEndLoad += UpdateColor;
		}
		
		private void OnDisable()
		{
			SaveSystem.OnEndLoad -= UpdateColor;
		}
		
		private void UpdateColor()
		{
			if(_stats._playerColor != default)
				SetPlayerColor(_stats._playerColor);
		}
		
		public void SetPlayerColor(Color color)
		{
			PlayerColor = color;
			_stats._playerColor = color;
			OnColorChange?.Invoke(PlayerColor);
		}
	}
}