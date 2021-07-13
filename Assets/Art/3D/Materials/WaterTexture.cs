using SoundSteppe.RefSystem;
using UnityEngine;

namespace HyperTetris
{
	public class WaterTexture : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private Vector2 _foamSpeed;
		[SerializeField, HideInInspector] private Renderer _renderer;
		private Material _material;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_renderer = gameObject.GetComponentInChildren<Renderer>();
		}
		
		private void Awake()
		{
			_material = _renderer.material;
		}
		
	    private void Update()
	    {
		    _material.mainTextureOffset += _foamSpeed * Time.deltaTime;
	    }
	}
}