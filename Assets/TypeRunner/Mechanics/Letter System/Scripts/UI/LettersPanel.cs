using System.Collections.Generic;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class LettersPanel : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private Transform _ungroupParent;
		[SerializeField] private SelectedLetters _selectedLetters;
		[SerializeField, HideInInspector] private LetterButton[] _buttons;
		[SerializeField, HideInInspector]private LetterWriteSystem _letterWriteSystem;
		private E_LetterType[] _word;
		private List<LetterButton> _selectedButtons;
		private List<E_LetterType> _selectedWord;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_buttons = GetComponentsInChildren<LetterButton>(true);
		}
		
		public void SetLetterWriteSystem(LetterWriteSystem letterWriteSystem)
		{
			_letterWriteSystem = letterWriteSystem;
		}
		
		private void OnEnable()
		{
			LetterData.Instance.OnLetterAdd += OnLetterAdd;
			LetterData.Instance.OnLetterReplace += OnLetterReplace;
			LetterButton.OnLetterSelect += OnLetterSelect;
		}
		
		private void OnDisable()
		{
			LetterData.Instance.OnLetterAdd -= OnLetterAdd;
			LetterData.Instance.OnLetterReplace -= OnLetterReplace;
			LetterButton.OnLetterSelect -= OnLetterSelect;
		}
		
		private void OnLetterSelect(LetterButton letterButton)
		{
			E_LetterType letter = letterButton.Letter;
			_selectedWord.Add(letter);
			_selectedButtons.Add(letterButton);
			if(_selectedWord.Count == _word.Length)
			{
				bool successful = false;
				if(CompareWords(_word, _selectedWord.ToArray()))
					successful = true;
				_letterWriteSystem.DisableWordWritter(successful);
			}
		}
		
		private bool CompareWords(E_LetterType[] word1, E_LetterType[] word2)
		{
			for(int i = 0; i < word1.Length; i++)
			{
				if(word1[i] != word2[i])
					return false;
			}
			return true;
		}
		
		public void Activate(E_LetterType[] word)
		{
			_word = word;
			_selectedWord = new List<E_LetterType>();
			_selectedButtons = new List<LetterButton>();
			_selectedLetters.SetActiveSlots(word.Length);
			foreach(var button in _buttons)
			{
				button.Enable();
			}
		}
		
		public void DisableSelected()
		{
			_word = null;
			foreach(var button in _buttons)
			{
				button.Disable();
			}
			foreach(var button in _selectedButtons)
			{
				LetterData.Instance.TryRemoveLetter(button.Letter);
				button.gameObject.SetActive(false);
			}
		}
		
		private void OnLetterAdd(E_LetterType letter)
		{
			for(int i = 0; i < _buttons.Length; i++)
			{
				if(_buttons[i].gameObject.activeSelf == false)
				{
					_buttons[i].gameObject.SetActive(true);
					_buttons[i].Init(letter, _selectedLetters, _ungroupParent);
					return;
				}
			}
		}
		
		public void OnLetterReplace(E_LetterType letter, E_LetterType newLetter)
		{
			for(int i = 0; i < _buttons.Length; i++)
			{
				if(_buttons[i].Letter == letter)
				{
					_buttons[i].Init(newLetter, _selectedLetters, _ungroupParent);
					return;
				}
			}
		}
		
		//private void OnLetterRemove(E_LetterType letter)
		//{
		//	for(int i = 0; i < _buttons.Length; i++)
		//	{
		//		if(_buttons[i].Letter == letter)
		//		{
		//			//LetterData.Instance.TryRemoveLetter(letter);
		//			//print(letter);
		//			_buttons[i].gameObject.SetActive(false);
		//			//print("not remove..");
		//			return;
		//		}
		//	}
		//}
	}
}