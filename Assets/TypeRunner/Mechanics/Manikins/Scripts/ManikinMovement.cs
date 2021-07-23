using SoundSteppe.RefSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace TypeRunner
{
	public class ManikinMovement : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		[SerializeField] private float _followSpeed = 120f;
		//[SerializeField] private float _strafeMultiplier = 1f;
		//[SerializeField] private float _strafeMaxSpeed = 1f;
		private float _beginPosX;
		private float _targetStrafePos;
		private bool _isJumped = false;
		private bool _canMove = true;
		public bool IsIndependetMovement { get; set; } = false;
		
		public static event System.Action<Vector3> OnBorderCollide;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
		}
		
		private void OnCollisionEnter(Collision collisionInfo)
		{
			if(collisionInfo.gameObject.TryGetComponent(out Border border))
			{
				OnBorderCollide?.Invoke(collisionInfo.GetContact(0).point);
			}
		}
		
		public void MoveToPoint(Vector3 point)
		{
			if(IsIndependetMovement)
				return;
			point.y = transform.position.y;
			Vector3 direction = point - transform.position;
			float distance = direction.magnitude;
			_rigi.AddForce(direction * _followSpeed, ForceMode.Acceleration);
		}
		
		private void FixedUpdate()
		{
			if(IsIndependetMovement)
			{
				Move();
			}
		}
		
		public void Move()
		{
			//if(_isJumped == true || _canMove == false)
			//	return;
			_rigi.AddForce(Vector3.forward * _followSpeed * 2f, ForceMode.Acceleration);
		}
	    
		public void Strafe(float strafe)
		{
			//_targetStrafePos = (strafe * _strafeMultiplier);
		}
		 
		public void InitStrafe()
		{
			//_beginPosX = transform.position.x;
		}
		 
		public void StopStrafe()
		{
			//_beginPosX = transform.position.x;
			//_targetStrafePos = 0f;
			//_rigi.velocity.Scale(MathfExtension.Vector3NotX);
		}
		
		private void OnJumpEnd() { _isJumped = false; }
		public void Jump(float distance = 1f)
		{
			_isJumped = true;
			_rigi.DOJump(transform.position + transform.forward * distance, 2f, 1, 1.5f)
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