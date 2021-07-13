using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class Shop : MonoBehaviour, INeedReference
	{
		[SerializeField, HideInInspector] private CoinManager _coins;
		public int UnlockCost = 750;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_coins = FindObjectOfType<CoinManager>(true);
		}
		
		public void TryUnlockSkin()
		{
			if(_coins.TrySpend(UnlockCost))
			{
				print("skin bought!");
			}
		}
	}
}