using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class VibrateButton : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private Sprite[] _sprites;
		[SerializeField, HideInInspector] private Image _icon;
	    
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_icon = gameObject.GetComponentInChildren<Image>();
		}
		
		private void Start() { UpdateIcon(); }
		private void OnEnable() { UpdateIcon(); }
		
		private void UpdateIcon()
		{
			if(Settings.Instance.Vibration)
				_icon.sprite = _sprites[1];
			else
				_icon.sprite = _sprites[0];
		}
		
		public void OnPress()
		{
			Settings.Instance.Vibration = !Settings.Instance.Vibration;
			UpdateIcon();
		}
	}
}