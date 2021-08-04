using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class Door : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		[SerializeField] bool _mirrorAnim = false;
		
		public void Trigger()
		{
			if(_mirrorAnim)
			{
				_animator.SetTrigger("Open");
			}
			else
			{
				_animator.SetTrigger("OpenM");
			}
		}
	}
}