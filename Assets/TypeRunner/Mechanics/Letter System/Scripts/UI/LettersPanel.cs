using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class LettersPanel : MonoBehaviour
	{
		//------FIELDS
		[SerializeField, HideInInspector] private LetterButton[] _buttons;
		
		//------METHODS
		[Button]
		private void UpdateReferences()
		{
			_buttons = GetComponentsInChildren<LetterButton>(true);
		}
		
		private void OnEnable()
		{
			LetterData.Instance.OnLetterAdd += OnLetterAdd;
			LetterData.Instance.OnLetterRemove += OnLetterRemove;
		}
		
		private void OnDisable()
		{
			LetterData.Instance.OnLetterAdd -= OnLetterAdd;
			LetterData.Instance.OnLetterRemove -= OnLetterRemove;
		}
		
		public void Activate()
		{
			foreach(var button in _buttons)
			{
				button.Enable();
			}
		}
		
		public void Disable()
		{
			foreach(var button in _buttons)
			{
				button.Disable();
			}
		}
		
		private void OnLetterAdd(E_LetterType letter)
		{
			for(int i = 0; i < _buttons.Length; i++)
			{
				if(_buttons[i].gameObject.activeSelf == false)
				{
					_buttons[i].gameObject.SetActive(true);
					_buttons[i].Init(letter);
					return;
				}
			}
		}
		
		private void OnLetterRemove(E_LetterType letter)
		{
			for(int i = 0; i < _buttons.Length; i++)
			{
				if(_buttons[i].Letter == letter)
				{
					_buttons[i].gameObject.SetActive(false);
					return;
				}
			}
		}
	}
}