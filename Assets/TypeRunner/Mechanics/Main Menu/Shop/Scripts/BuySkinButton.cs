using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class BuySkinButton : MonoBehaviour, INeedReference
	{
		[SerializeField] private TMP_Text _costText;
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private Shop _shop;
		//private int _reward;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_coins = FindObjectOfType<CoinManager>(true);
				_shop = FindObjectOfType<Shop>(true);
			}
		}
		
		private void OnEnable()
		{
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			_costText.text = "OPEN:\n" + _shop.UnlockCost.ToString();
		}
		
		public void TryBuy()
		{
			_shop.TryUnlockSkin();
			UpdateUI();
		}
	}
}