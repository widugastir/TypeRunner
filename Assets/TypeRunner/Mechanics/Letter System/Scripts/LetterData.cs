using UnityEngine;

namespace TypeRunner
{
	public class LetterData : Singleton<LetterData>
	{
		//------FIELDS
		[SerializeField] private string _letters = "";
		
		//------METHODS
		public void AddLetter(string letter)
		{
			_letters += letter;
		}
		
		public bool TryRemoveLetter(string letter)
		{
			if(_letters.Contains(letter))
			{
				int index = _letters.IndexOf(letter);
				_letters = _letters.Remove(index, letter.Length);
				return true;
			}
			return false;
		}
		
		public bool Contain(string letter)
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