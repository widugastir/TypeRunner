using UnityEngine;

namespace TypeRunner
{
	public class FlySystem : MonoBehaviour
	{
		[SerializeField] private LetterWriteSystem _letterSystem;
		[SerializeField] private GameObject _panel;
		[SerializeField] private LetterButton[] _buttons;
		[SerializeField] private E_LetterType[] _requiredWord;
		[SerializeField] private SelectedLetters _selectedLetters;
		[SerializeField] private Transform _ungroupParent;
		
		public void Enable()
		{
			_panel.SetActive(true);
			_letterSystem.Disable();
		}
		
		public void Disable()
		{
			_panel.SetActive(false);
			_letterSystem.Enable();
		}
		
		private void Init()
		{
			for(int i = 0; i < _buttons.Length; i++)
			{
				if(i >= _requiredWord.Length)
					break;
				_buttons[i].Init(_requiredWord[i], _selectedLetters, _ungroupParent);
			}
		}
	}
}