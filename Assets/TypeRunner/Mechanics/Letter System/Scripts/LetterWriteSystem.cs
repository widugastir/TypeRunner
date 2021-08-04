﻿using SoundSteppe.RefSystem;
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
			StartCoroutine(LateEnableWriter(zone._requiredWord));
		}
		
		private IEnumerator LateEnableWriter(E_LetterType[] word)
		{
			yield return null;
			EnableWordWritter(word);
		}
		
		private void PlayerEnterZone(ObstacleZone zone, E_LetterType[] word)
		{
			EnableWordWritter(zone, word);
		}
		
		private void PlayerExitZone(ObstacleZone zone)
		{
			DisableWordWritter(false, false, zone);
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
			_boost.EnableTimer(DisableWordWritter);
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
				
			_successfulAmount.text = $"X{_stats.SuccessfulMultiplier.ToString()}";
			
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
			_uiPanel.SetActive(true);
		}
		
		public void Disable()
		{
			_uiPanel.SetActive(false);
		}
		
		public void Reset()
		{
			_uiPanel.SetActive(true);
			_boostObject.SetActive(false);
			_lettersPanel.Reset();
		}
	}
}