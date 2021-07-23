﻿using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class LetterWriteSystem : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private LayoutGroup _layoutGroup;
		[SerializeField] private TMP_Text _wordText;
		[SerializeField] private GameObject _uiPanel;
		[SerializeField] private int _manikinsDieOnLose = 1;
		[SerializeField] private float _timeScale = 0.2f;
		[SerializeField] private PlayerController _playerController;
		[SerializeField, HideInInspector] private LettersPanel _lettersPanel;
		private bool _isReady = true;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_lettersPanel = gameObject.GetComponentInChildren<LettersPanel>();
			_lettersPanel.SetLetterWriteSystem(this);
		}
		
		private void OnEnable()
		{
			ObstacleZone.OnPlayerEntered += PlayerEnterZone;
			ObstacleZone.OnPlayerExit += PlayerExitZone;
		}
		
		private void OnDisable()
		{
			ObstacleZone.OnPlayerEntered -= PlayerEnterZone;
			ObstacleZone.OnPlayerExit -= PlayerExitZone;
		}
		
		private void PlayerEnterZone(ObstacleZone zone, E_LetterType[] word)
		{
			EnableWordWritter(zone, word);
		}
		
		private void PlayerExitZone(ObstacleZone zone)
		{
			DisableWordWritter(false);
		}
		
		private void EnableWordWritter(ObstacleZone zone, E_LetterType[] word)
		{
			_layoutGroup.enabled = false;
			_wordText.gameObject.SetActive(true);
			string req_word = "";
			foreach(var ch in word)
			{
				req_word += ch.ToString();
			}
			_wordText.text = req_word.ToUpper();
			_isReady = true;
			Time.timeScale = _timeScale;
			_lettersPanel.Activate(word);
		}
		
		public void DisableWordWritter(bool successful)
		{
			if(_isReady == false)
				return;
			_layoutGroup.enabled = true;
			_wordText.gameObject.SetActive(false);
			_isReady = false;
			if(successful == false)
			{
				//One manikin die =(
				_playerController.BlockManikins(_manikinsDieOnLose);
			}
			Time.timeScale = 1f;
			_lettersPanel.DisableSelected();
		}
		
		public void Reset()
		{
			_uiPanel.SetActive(true);
			_lettersPanel.Reset();
		}
	}
}