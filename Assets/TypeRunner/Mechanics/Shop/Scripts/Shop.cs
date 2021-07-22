using System.Collections.Generic;
using System.Linq;
using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class Shop : MonoBehaviour, INeedReference
	{
		[SerializeField, HideInInspector] private CoinManager _coins;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField] private int _costPerSkin = 250;
		[SerializeField] private int _unlockCost = 500;
		public int UnlockCost
		{
			get 
			{
				return _unlockCost + _costPerSkin * BoughtedSkins();
			}
			
			private set
			{
				_unlockCost = value;
			}
		}
		
		public static event System.Action OnSkinSelect;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_coins = FindObjectOfType<CoinManager>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
			}
		}
		
		private int BoughtedSkins() => _stats.PurchasedSkins.Count;
		
		public void TryUnlockSkin()
		{
			if(_coins.TrySpend(UnlockCost))
			{
				SelectSkin(UnlockRandomSkin());
				//SelectSkin();
			}
		}
		
		public bool IsAllSkinBoughted()
		{
			int amount = System.Enum.GetValues(typeof(SkinType)).Length;
			for(int i = 0; i < amount; i++)
			{
				if(_stats.PurchasedSkins.Contains((SkinType)i) == false)
					return false;
			}
			return true;
		}
		
		public SkinType UnlockRandomSkin()
		{
			int amount = System.Enum.GetValues(typeof(SkinType)).Length;
			List<SkinType> skins = new List<SkinType>();
			for(int i = 0; i < amount; i++)
			{
				if(_stats.PurchasedSkins.Contains((SkinType)i))
					continue;
				skins.Add((SkinType)i);
			}
			
			if(skins.Count == 0)
				return default;
			int rand = Random.Range(0, skins.Count);
			_stats.PurchasedSkins.Add(skins[rand]);
			return skins[rand];
		}
		
		public void SelectSkin(SkinType skin)
		{
			_stats._playerSkin = skin;
			OnSkinSelect?.Invoke();
		}
	}
}