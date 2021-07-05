using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class SelectedLetters : MonoBehaviour
	{
		//------FIELDS
		[SerializeField, HideInInspector] private List<Transform> _letterPositions;
		private int _lastSlot = 0;
		
		//------METHODS
		[Button]
		private void UpdateReferences()
		{
			_letterPositions = new List<Transform>();
			foreach(Transform child in transform)
			{
				_letterPositions.Add(child);
			}
		}
		
		public void SetActiveSlots(int amount)
		{
			_lastSlot = 0;
			for(int i = 0; i < _letterPositions.Count; i++)
			{
				if(amount > 0)
				{
					amount--;
					_letterPositions[i].gameObject.SetActive(true);
					continue;
				}
				_letterPositions[i].gameObject.SetActive(false);
			}
		}
		
		public Vector3 GetLetterPosition()
		{
			var position = _letterPositions[_lastSlot].position;
			_lastSlot++;
			return position;
		}
	}
}