using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;

namespace TypeRunner
{
	public class LetterWriteSystem : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private LayoutGroup _layoutGroup;
		[SerializeField] private TMP_Text _wordText;
		[SerializeField] private TMP_Text _successfulAmount;
		[SerializeField] private GameObject _uiPanel;
		[SerializeField] private PlayerStats _stats;
		[SerializeField] private PlayerController _playerController;
		
		[SerializeField] private GameObject _boostObject;
		[SerializeField] private Boost _boost;
		
		[SerializeField, HideInInspector] private LettersPanel _lettersPanel;
		private bool _isReady = true;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_lettersPanel = gameObject.GetComponentInChildren<LettersPanel>(true);
			_lettersPanel.SetLetterWriteSystem(this);
		}
		
		private void OnEnable()
		{
			ObstacleZone.OnPlayerEntered += PlayerEnterZone;
			FlyZone.EnterFlyZone += EnterFlyZone;
			ObstacleZone.OnPlayerExit += PlayerExitZone;
		}
		
		private void OnDisable()
		{
			ObstacleZone.OnPlayerEntered -= PlayerEnterZone;
			FlyZone.EnterFlyZone -= EnterFlyZone;
			ObstacleZone.OnPlayerExit -= PlayerExitZone;
		}
		
		private void EnterFlyZone(FlyZone zone)
		{
			_playerController.SetMansSuccesfull(false);
			foreach(var l in zone._requiredWord)
			{
				LetterData.Instance.AddLetter(l);
			}
			StartCoroutine(LateEnableWriter(zone));
		}
		
		private IEnumerator LateEnableWriter(FlyZone zone)
		{
			yield return null;
			EnableWordWritter(zone, zone._requiredWord);
		}
		
		private void PlayerEnterZone(ObstacleZone zone, E_LetterType[] word)
		{
			EnableWordWritter(zone, word);
		}
		
		private void PlayerExitZone(ObstacleZone zone)
		{
			DisableWordWritter(false, false, zone);
		}
		
		private void EnableWordWritter(FlyZone zone, E_LetterType[] word)
		{
			_playerController.SetMansSuccesfull(false);
			_layoutGroup.enabled = false;
			_wordText.gameObject.SetActive(true);
			_boostObject.SetActive(true);
			string req_word = "";
			foreach(var ch in word)
			{
				req_word += ch.ToString();
			}
			_wordText.text = req_word.ToUpper();
			_isReady = true;
			_boost.EnableTimer(zone, DisableWordWritter);
			Time.timeScale = zone._timeScale;
			_lettersPanel.Activate(word);
		}
		
		private void EnableWordWritter(ObstacleZone zone, E_LetterType[] word)
		{
			_playerController.SetMansSuccesfull(false);
			_layoutGroup.enabled = false;
			_wordText.gameObject.SetActive(true);
			_boostObject.SetActive(true);
			string req_word = "";
			foreach(var ch in word)
			{
				req_word += ch.ToString();
			}
			_wordText.text = req_word.ToUpper();
			_isReady = true;
			_boost.EnableTimer(zone, DisableWordWritter);
			Time.timeScale = zone._timeScale;
			_lettersPanel.Activate(word);
		}
		
		public void DisableWordWritter(bool successful, bool bonus = false, ObstacleZone zone = null)
		{
			if(_isReady == false)
				return;
			_layoutGroup.enabled = true;
			_wordText.gameObject.SetActive(false);
			_boostObject.SetActive(false);
			_isReady = false;
			
			_stats._successfulWord += (successful ? +1 : -99999);
			if(_stats._successfulWord < 0)
				_stats._successfulWord = 0;
			
			_boost.StopTimer();
			if(successful)
			{
				if(_boost.Result == Boost.BoostTimerResult.bonus)
				{
					_boost.ApplyBonus();
				}
			}
				
			UpdateUI();
			
			_playerController.SetMansSuccesfull(successful);
			if(successful == false)
			{	
				if(zone != null)
					_playerController.BlockManikins(zone._manikinsToBlock);
				else
					_playerController.BlockManikins(1);
			}
			Time.timeScale = 1f;
			_lettersPanel.DisableSelected();
		}
		
		public void EnableOutline()
		{
			_lettersPanel.EnableOutline();
		}
		
		public void DisableOutline()
		{
			_lettersPanel.DisableOutline();
		}
		
		private void UpdateUI()
		{
			_successfulAmount.text = $"x{_stats.SuccessfulMultiplier.ToString()}";
		}
		
		private void EnableWordWritter(E_LetterType[] word)
		{
			_playerController.SetMansSuccesfull(false);
			_layoutGroup.enabled = false;
			_wordText.gameObject.SetActive(true);
			_boostObject.SetActive(true);
			string req_word = "";
			foreach(var ch in word)
			{
				req_word += ch.ToString();
			}
			_wordText.text = req_word.ToUpper();
			_isReady = true;
			_boost.EnableTimer(DisableWordWritter);
			_lettersPanel.Activate(word);
		}
		
		public void DisableWordWritter()
		{
			if(_isReady == false)
				return;
			_layoutGroup.enabled = true;
			_lettersPanel.DisableSelected();
			_layoutGroup.enabled = true;
			_wordText.gameObject.SetActive(false);
			_boostObject.SetActive(false);
			_isReady = false;
			Time.timeScale = 1f;
			_lettersPanel.DisableSelected();
			_layoutGroup.enabled = true;
			//_lettersPanel.Reset();
		}
		
		public void Enable()
		{
			UpdateUI();
			_lettersPanel.gameObject.SetActive(true);
			_uiPanel.SetActive(true);
		}
		
		public void Disable()
		{
			_lettersPanel.gameObject.SetActive(false);
			_uiPanel.SetActive(false);
			Reset();
		}
		
		public void Reset()
		{
			_lettersPanel.gameObject.SetActive(true);
			_uiPanel.SetActive(true);
			_boostObject.SetActive(false);
			_lettersPanel.Reset(true);
			//_stats._successfulWord = 0;
			UpdateUI();
		}
	}
}