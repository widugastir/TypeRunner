﻿using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace TypeRunner
{
	public class LetterButton : MonoBehaviour
	{
		//------FIELDS
		[SerializeField, HideInInspector] private TMP_Text _text;
		[SerializeField, HideInInspector] private Button _button;
		[HideInInspector] public E_LetterType Letter;
		private SelectedLetters _selectedLetters;
		private Transform _ungroupParent;
		[SerializeField] private Transform _startParent;
		
		public static event System.Action<LetterButton> OnLetterSelect;
		
		//------METHODS
		[Button]
		private void UpdateReferences()
		{
			_button = GetComponentInChildren<Button>(true);
			_text = GetComponentInChildren<TMP_Text>(true);
		}
		
		private void OnDisable()
		{
			transform.DOKill();
		}
		
		public void Init(E_LetterType letter, SelectedLetters selectedLetters, Transform ungroupParent)
		{
			Letter = letter;
			_selectedLetters = selectedLetters;
			_ungroupParent = ungroupParent;
			_text.text = letter.ToString().ToUpper();
		}
		
		public void Enable()
		{
			_button.interactable = true;
		}
		
		public void Disable()
		{
			_button.interactable = false;
			transform.SetParent(_startParent);
			//print(Letter + "_____" + _startParent + "    " + transform.parent);
			//gameObject.SetActive(false);
		}
		
		public void OnPress()
		{
			transform.SetParent(_ungroupParent);
			Vector3 newPos = _selectedLetters.GetLetterPosition();
			MoveTo(newPos);
			_button.interactable = false;
		}
		
		private void MoveTo(Vector3 position)
		{
			transform.DOLocalMove(position, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(OnAnimComplete);
		}
		
		private void OnAnimComplete()
		{
			OnLetterSelect?.Invoke(this);
		}
	}
}