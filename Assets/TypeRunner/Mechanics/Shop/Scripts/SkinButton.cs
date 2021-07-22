using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class SkinButton : MonoBehaviour, INeedReference
	{
		[SerializeField] private SkinType _skin;
		[SerializeField] private Image _image;
		[SerializeField] private GameObject _skinImage;
		[SerializeField] private GameObject _lockImage;
		[SerializeField] private Sprite _selected;
		[SerializeField] private Sprite _default;
		
		[SerializeField, HideInInspector] private Shop _shop;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_shop = FindObjectOfType<Shop>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
			}
		}
		
		private void OnEnable()
		{
			UpdateUI();
			Shop.OnSkinSelect += SkinSelected;
		}
		
		private void OnDisable()
		{
			Shop.OnSkinSelect -= SkinSelected;
		}
		
		private void SkinSelected()
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			if(_stats._playerSkin == _skin)
				_image.sprite = _selected;
			else
				_image.sprite = _default;
				
			if(_stats.PurchasedSkins.Contains(_skin))
			{
				_skinImage.SetActive(true);
				_lockImage.SetActive(false);
			}
			else
			{
				_skinImage.SetActive(false);
				_lockImage.SetActive(true);
			}
		}
		
		public void SelectSkin()
		{
			if(_stats.PurchasedSkins.Contains(_skin))
		    	_shop.SelectSkin(_skin);
	    }
	}
}