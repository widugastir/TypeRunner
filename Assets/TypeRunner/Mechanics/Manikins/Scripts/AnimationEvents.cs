using UnityEngine;

namespace TypeRunner
{
	public class AnimationEvents : MonoBehaviour
	{
		public event System.Action OnAnimationEnd;
		public event System.Action OnThrow;
		
		public void AnimationEnd() 
		{
			OnAnimationEnd?.Invoke();
		}
		
		public void Throw() 
		{
			OnThrow?.Invoke();
		}
	}
}