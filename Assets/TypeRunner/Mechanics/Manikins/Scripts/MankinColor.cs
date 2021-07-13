using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MankinColor : MonoBehaviour, INeedReference
	{
		private ColorChanger _colorChanger;
		[SerializeField, HideInInspector] private Renderer _renderer;
		
		public void UpdateReferences(bool sceneObject)
		{
			_renderer = gameObject.GetComponentInChildren<Renderer>();
		}
		
		private void OnEnable()
		{
			ColorChanger.OnColorChange += SetColor;
		}
		
		private void OnDisable()
		{
			ColorChanger.OnColorChange -= SetColor;
		}
		
		private void Start()
		{
			_colorChanger = FindObjectOfType<ColorChanger>();
			SetColor(_colorChanger.PlayerColor);
		}
		
		public void SetColor(Color color)
		{
			_renderer.material.color = color;
		}
	}
}