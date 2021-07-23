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
		[SerializeField] private float _strafeSpeed = 3f;
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
			bool strafeRight = point.x > transform.position.x ? true : false;
			float distanceX = Mathf.Abs(point.x - transform.position.x);
			point.y = transform.position.y;
			point.x = transform.position.x;
			Vector3 direction = point - transform.position;
			float distance = direction.magnitude;
			
			//point.z = transform.position.z;
			//point.x = transform.position.x;
			//Vector3 distanceZ = point - transform.position;
			
			//print($"{distanceZ.magnitude}");
			//Vector3 newDir = new Vector3(0f, 0f, distanceZ.magnitude * _followSpeed);
			//_rigi.MovePosition(transform.position + newDir * Time.deltaTime);
			_rigi.AddForce(direction * _followSpeed, ForceMode.Acceleration);
			if(distanceX >= 0.5f)
				_rigi.AddForce((strafeRight ? Vector3.right : Vector3.left) * distanceX * _strafeSpeed, ForceMode.Acceleration);
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