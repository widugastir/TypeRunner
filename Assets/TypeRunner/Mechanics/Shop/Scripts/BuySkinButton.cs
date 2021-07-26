using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class BuySkinButton : MonoBehaviour, INeedReference
	{
		[SerializeField] private TMP_Text _costText;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private Button _button;
		[SerializeField, HideInInspector] private Shop _shop;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_coins = FindObjectOfType<CoinManager>(true);
				_shop = FindObjectOfType<Shop>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_button = GetComponentInChildren<Button>();
			}
		}
		
		private void OnEnable()
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			if(_shop.IsAllSkinBoughted())
			{
				_costText.text = "All boughted!";
				_button.interactable = false;
			}
			else
			{
				_costText.text = "OPEN:\n" + _shop.UnlockCost.ToString();
				if(_stats.Coins >= _shop.UnlockCost)
					_button.interactable = true;
				else
					_button.interactable = false;
			}
		}
		
		public void TryBuy()
		{
			if(_shop.IsAllSkinBoughted())
				return;
			_shop.TryUnlockSkin();
			UpdateUI();
		}
	}
}