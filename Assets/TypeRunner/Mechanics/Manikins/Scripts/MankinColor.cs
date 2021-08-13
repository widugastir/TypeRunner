using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MankinColor : MonoBehaviour, INeedReference
	{
		private ColorChanger _colorChanger;
		[SerializeField, HideInInspector] private SkinChanger _skins;
		[SerializeField] private ParticleSystem[] _particles;
		private Color mainColor;
		
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == false)
			{
				_skins = GetComponentInChildren<SkinChanger>(true);
			}
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
			UpdateColor();
		}
		
		private void SetColor(Color color)
		{
			UpdateColor();
		}
		
		public void UpdateColor()
		{
			if(_colorChanger == null)
				_colorChanger = FindObjectOfType<ColorChanger>();
			Color newColor = _colorChanger.PlayerColor;
			foreach(var p in _particles)
			{
				p.startColor = newColor;
			}
			Renderer renderer = _skins._current._renderer;
			for(int i = 0; i < renderer.materials.Length; i++)
			{
				if(i == _skins._current.MainMaterialIndex)
				{
					renderer.materials[i].color = newColor;
				}
				else
				{
					renderer.materials[i].color = newColor * 0.5f;
					
				}
			}
		}
	}
}