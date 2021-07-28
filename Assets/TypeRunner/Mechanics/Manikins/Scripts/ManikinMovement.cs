﻿using SoundSteppe.RefSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace TypeRunner
{
	public class ManikinMovement : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		[SerializeField] private float _strafeSpeed = 3f;
		[SerializeField] private float _forwardSpeed = 3f;
		private bool _isJumped = false;
		private bool _canMove = true;
		public bool IsIndependetMovement { get; set; } = false;
		public bool MoveToGroupCenter { get; set; } = true;
		private bool CanMoveForward{ get; set; } = false;
		private Tween _goToTween;
		private Transform _groupCenter;
		
		public static event System.Action<Vector3> OnBorderCollide;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
		}
		
		public void Init(Transform groupCenter)
		{
			_groupCenter = groupCenter;
		}
		
		public void SetCanMove2(bool b)
		{
			CanMoveForward = b;
		}
		
		protected void OnDisable()
		{
			if(_goToTween != null)
			{
				_goToTween.Kill();
				_goToTween = null;
			}
		}
		
		public void GoTo(Vector3 position, float duration)
		{
			if(_goToTween != null)
			{
				_goToTween.Kill();
				_goToTween = null;
			}
			_goToTween = _rigi
				.DOMove(position, duration)
				.SetEase(Ease.Linear);
		}
		
		private void MoveToPoint()
		{
			if(IsIndependetMovement || _groupCenter == null || MoveToGroupCenter == false)
				return;
			Vector3 force = (_groupCenter.position - _rigi.position).normalized * _strafeSpeed;
			_rigi.AddForce(force, ForceMode.Acceleration);
		}
		
		private void MoveForward()
		{
			if(CanMoveForward == false)
				return;
			//print(123);
			_rigi.AddForce(Vector3.forward * _forwardSpeed, ForceMode.Acceleration);
		}
		
		private void FixedUpdate()
		{
			MoveToPoint();
			MoveForward();
		}
		
		private void OnJumpEnd() 
		{ 
			_rigi.useGravity = true;
			_isJumped = false; 
		}
		
		public void Jump(float height, float time)
		{
			_rigi.useGravity = false;
			_isJumped = true;
			_rigi.DOJump(transform.position, height, 1, time)
				.SetEase(Ease.Linear)
				.OnComplete(OnJumpEnd);
		}
		
		public void SetCanMove(bool canMove)
		{
			_canMove = canMove;
		}
		
		public void PushAway(Vector3 point, float distance = 0.03f)
		{
			Vector3 direction = transform.position - point;
			transform.position += direction.normalized * distance;
		}
	}
}