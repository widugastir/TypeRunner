using UnityEngine;
using DG.Tweening;

namespace TypeRunner
{
	public class SkinChanger : MonoBehaviour
	{
		[SerializeField] private Skin[] _skins;
		[SerializeField] public Skin _current;
		[SerializeField] public MankinColor _manColor;
		private Tween _flickering = null;
		
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
			StopFlickering();
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
		
		public void StartFlickering()
		{
			StopFlickering();
			if(_flickering != null)
				return;
			_flickering = _current.renderer.material
				.DOFade(0f, "_Color", 0.1f)
				.SetEase(Ease.Linear)
				.SetLoops(-1, LoopType.Yoyo)
				.SetUpdate(true)
				.OnComplete(OnFlickerStop);
		}
		
		public void StopFlickering()
		{
			if(_flickering == null)
				return;
			_flickering.Kill(true);
			_flickering = null;
		}
		
		private void OnFlickerStop()
		{
			Color color = _current.renderer.material.color;
			color.a = 1f;
			_current.renderer.material.color = color;
			_flickering = null;
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