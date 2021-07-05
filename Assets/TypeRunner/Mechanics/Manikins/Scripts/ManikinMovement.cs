﻿using UnityEngine;
using DG.Tweening;

namespace TypeRunner
{
	public class ManikinMovement : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private Rigidbody _rigi;
		[SerializeField] private float _strafeMultiplier = 1f;
		[SerializeField] private float _strafeMaxSpeed = 1f;
		private float _beginPosX;
		private float _targetStrafePos;
		private bool _isJumped = false;
		
		public static event System.Action<Vector3> OnBorderCollide;
		
		//------METHODS
		private void OnCollisionEnter(Collision collisionInfo)
		{
			if(collisionInfo.gameObject.TryGetComponent(out Border border))
			{
				OnBorderCollide?.Invoke(collisionInfo.GetContact(0).point);
			}
		}
		
		public void Move(Vector3 force)
		{
			if(_isJumped == true)
				return;
			Vector3 moveDirection = force;
			if(_targetStrafePos != 0f)
			{
				moveDirection += ((transform.position + transform.right * _targetStrafePos) - transform.position);// transform.right * _targetPosX;
				if(moveDirection.x > _strafeMaxSpeed)
					moveDirection.x = _strafeMaxSpeed;
				if(moveDirection.x < -_strafeMaxSpeed)
					moveDirection.x = -_strafeMaxSpeed;
			}
			_rigi.MovePosition(transform.position + moveDirection * Time.deltaTime);
		}
	    
		public void Strafe(float strafe)
		{
			_targetStrafePos = (strafe * _strafeMultiplier);
		}
		 
		public void InitStrafe()
		{
			_beginPosX = transform.position.x;
		}
		 
		public void StopStrafe()
		{
			_beginPosX = transform.position.x;
			_targetStrafePos = 0f;
			_rigi.velocity.Scale(MathfExtension.Vector3NotX);
		}
		
		public void Jump(float distance = 1f)
		{
			_isJumped = true;
			_rigi.DOJump(transform.position + transform.forward * distance, 2f, 1, 1.5f)
				.SetEase(Ease.Linear)
				.OnComplete(OnJumpEnd);
				
			void OnJumpEnd()
			{
				_isJumped = false;
			}
		}
		
		public void PushAway(Vector3 point, float distance = 0.03f)
		{
			Vector3 direction = transform.position - point;
			transform.position += direction.normalized * distance;
		}
	}
}