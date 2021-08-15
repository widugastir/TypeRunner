using SoundSteppe.RefSystem;
using UnityEngine;
using System;

namespace TypeRunner
{
	public class CinematicSystem : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private MultiplierAnimations _mpAnimations;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				
			}
		}
		
		public void PlayMultiplyView(Action onMultiplyAnim)
		{
			_mpAnimations.PlayNumericAnim(onMultiplyAnim);
		}
	}
}