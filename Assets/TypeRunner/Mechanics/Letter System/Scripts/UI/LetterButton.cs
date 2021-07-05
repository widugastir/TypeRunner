using NaughtyAttributes;
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
		
		//------METHODS
		[Button]
		private void UpdateReferences()
		{
			_button = GetComponentInChildren<Button>(true);
			_text = GetComponentInChildren<TMP_Text>(true);
		}
		
		public void Init(E_LetterType letter)
		{
			Letter = letter;
			_text.text = letter.ToString().ToUpper();
		}
		
		public void Enable()
		{
			_button.interactable = true;
		}
		
		public void Disable()
		{
			_button.interactable = false;
		}
		
		public void OnPress()
		{
			//MOVE TO
			print("AAAAA");
		}
	}
}