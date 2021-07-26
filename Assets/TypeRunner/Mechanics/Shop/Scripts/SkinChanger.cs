using UnityEngine;

namespace TypeRunner
{
	public class SkinChanger : MonoBehaviour
	{
		[SerializeField] private Skin[] _skins;
		[SerializeField] public Skin _current;
		[SerializeField] public MankinColor _manColor;
		private PlayerStats _stats;
		private PlayerStats Stats 
		{
			get 
			{
				if(_stats == null)
				{
					_stats = FindObjectOfType<PlayerStats>(true);
				}
				return _stats;
			}
			set 
			{
				_stats = value;
			}
		}
		
		private void OnEnable()
		{
			Shop.OnSkinSelect += SetSkin;
			SaveSystem.OnEndLoad += UpdateSkin;
		} 
		
		private void OnDisable()
		{
			Shop.OnSkinSelect -= SetSkin;
			SaveSystem.OnEndLoad -= UpdateSkin;
		}
		
		private void Start()
		{
			UpdateSkin();
		}
		
		private void UpdateSkin()
		{
			SetSkin();
		}
		
		public void SetSkin()
		{
			SetSkin(Stats._playerSkin);
			_manColor.UpdateColor();
		}
		
		public void SetSkin(SkinType type)
		{
			//if(_current == null)
				//return;
				
		    if(_current != null)
		    	_current._skinObject.SetActive(false);
		    
			foreach(var s in _skins)
			{
				if(s._type == type)
				{
					_current = s;
					_current._skinObject.SetActive(true);
					break;
				}
			}
	    }
	}
}