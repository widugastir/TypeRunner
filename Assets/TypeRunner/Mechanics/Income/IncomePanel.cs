using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class IncomePanel : MonoBehaviour, INeedReference
	{
		[SerializeField] private TMP_Text _coinsText;
		[SerializeField] private GameObject _holder;
		[SerializeField, HideInInspector] private CoinManager _coins;
		private int _reward;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
				_coins = FindObjectOfType<CoinManager>(true);
		}
		
		public void Enable(int reward)
		{
			_reward = reward;
			_coinsText.text = _reward.ToString();
			_holder.SetActive(true);
		}
		
		public void GainCoins()
		{
			_coins.AddCoins(_reward);
			_holder.SetActive(false);
		}
	}
}