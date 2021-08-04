using SoundSteppe.RefSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class ClaimButton : MonoBehaviour, INeedReference
	{
		[SerializeField] private float _closeRouletteDelay = 1.5f;
		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _coins;
		[SerializeField] private LevelRoulette _roulette;
		
		[SerializeField] private GameEndPanel _endPanel;
		[SerializeField] private GameObject _mainMenuCanvas;
		
		[SerializeField, HideInInspector] private PlayerStats _stats;
		//[SerializeField] private DisplayCoins _displayCoins;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_button = GetComponentInChildren<Button>();
			}
		}
		
		public void TryClaim()
		{
			_roulette.Roll(OnRollEnd);
			_button.interactable = false;
		}
		
		private void OnRollEnd(float reward)
		{
			_stats._coinsMultiplier = reward;
			_coins.text = "+" + Mathf.RoundToInt(_stats.EarnedCoins * _stats._coinsMultiplier * _stats.SuccessfulMultiplier).ToString();
			StartCoroutine(CloseRoulette(reward));
		}
		
		private IEnumerator CloseRoulette(float reward)
		{
			yield return new WaitForSecondsRealtime(_closeRouletteDelay);
			_endPanel.Disable();
			_mainMenuCanvas.SetActive(true);
		}
	}
}