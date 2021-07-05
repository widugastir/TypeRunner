using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class LetterData : Singleton<LetterData>
	{
		//------FIELDS
		[SerializeField] private int _maxLetters = 6;
		[SerializeField, ReadOnly] private List<E_LetterType> _letters = new List<E_LetterType>();
		
		public event System.Action<E_LetterType> OnLetterAdd;
		public event System.Action<E_LetterType> OnLetterRemove;
		public event System.Action<E_LetterType, E_LetterType> OnLetterReplace;
		
		//------METHODS
		public void AddLetter(E_LetterType letter)
		{
			if(_letters.Count >= _maxLetters)
			{
				var l = _letters[0];
				_letters.RemoveAt(0);
				OnLetterRemove?.Invoke(l);
				OnLetterReplace?.Invoke(l, letter);
			}
			_letters.Add(letter);
			OnLetterAdd?.Invoke(letter);
		}
		
		public bool TryRemoveLetter(E_LetterType letter)
		{
			if(_letters.Contains(letter))
			{
				OnLetterRemove?.Invoke(letter);
				_letters.Remove(letter);
				return true;
			}
			return false;
		}
		
		public bool Contain(E_LetterType letter)
		{
			if(_letters.Contains(letter))
				return true;
			return false;
		}
	}
	
	public enum E_LetterType
	{
		J, U, M, P, T, H, R,
		O, W, S, L, I, D, E
	}
}