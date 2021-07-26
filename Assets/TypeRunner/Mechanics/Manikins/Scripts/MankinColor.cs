using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class MankinColor : MonoBehaviour, INeedReference
	{
		private ColorChanger _colorChanger;
		[SerializeField, HideInInspector] private SkinChanger _skins;
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
			_colorChanger = FindObjectOfType<ColorChanger>();
			UpdateColor();
		}
		
		private void SetColor(Color color)
		{
			UpdateColor();
		}
		
		public void UpdateColor()
		{
			Renderer renderer = _skins._current.renderer;
			for(int i = 0; i < renderer.materials.Length; i++)
			{
				if(i == _skins._current.MainMaterialIndex)
				{
					renderer.materials[i].color = _colorChanger.PlayerColor;
				}
				else
				{
					renderer.materials[i].color = _colorChanger.PlayerColor * 0.5f;
					
				}
			}
		}
	}
}