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
		
		public Vector3 _basePosition;
		public void InitStrafeToPoint(Vector3 position)
		{
			_basePosition = position;
		}
		
		public void MoveOffsetBy(Vector3 position)
		{
			
		}
		
		//Late update
		public void StrafeToPoint(Vector3 point)
		{
			//print(point);
			//Vector3 force = (point + _targetOffset) - transform.position;
			//force *= _strafeSpeed * Time.deltaTime;
			//transform.position = point + _targetOffset;
			//_rigi.MovePosition( point + _targetOffset );
			//transform.Translate(force);
			
			
			Vector3 direction = point - transform.position;
			direction *= _strafeSpeed * Time.deltaTime;
			//_rigi.MovePosition(_rigi.position + direction);
		}
		
		//Fixed update
		public void MoveToPoint(Vector3 point)
		{
			if(IsIndependetMovement)
				return;
			bool strafeRight = point.x > transform.position.x ? true : false;
			float distanceX = Mathf.Abs(point.x - transform.position.x);
			
			//_rigi.AddForce((point - _rigi.position).normalized * _strafeSpeed, ForceMode.Acceleration);
			
			point.y = transform.position.y;
			point.x = transform.position.x;
			Vector3 direction = point - transform.position;
			float distance = direction.magnitude;
			float distanceZ = Mathf.Abs(point.z - transform.position.z);
			
			Vector3 targetPos = _rigi.position;
			if(distanceZ >= 0.5f)
				targetPos += Vector3.forward * _followSpeed * Time.deltaTime;
			//if(distanceX >= 0.3f)
			//	targetPos += (_targetOffset - _rigi.position) * _strafeSpeed * Time.deltaTime;
			//if(distanceX >= 0.5f)
				//targetPos += (strafeRight ? Vector3.right : Vector3.left) * _strafeSpeed * Time.deltaTime;
			//_rigi.MovePosition(targetPos);
			
			
			Vector3 force = Vector3.forward * _followSpeed;
			
			_rigi.AddForce(force, ForceMode.Acceleration);
			
			
			
			//if(distanceZ >= 0.5f)
			//	_rigi.MovePosition(_rigi.position + Vector3.forward * _followSpeed * Time.deltaTime);
			//if(distanceX >= 0.5f)
			//	_rigi.MovePosition(_rigi.position + (strafeRight ? Vector3.right : Vector3.left) * _strafeSpeed * Time.deltaTime);
			
			//_rigi.AddForce(Vector3.forward * _followSpeed * 2f, ForceMode.Acceleration);
			
			//point.z = transform.position.z;
			//point.x = transform.position.x;
			//Vector3 distanceZ = point - transform.position;
			
			//print($"{distanceZ.magnitude}");
			//Vector3 newDir = new Vector3(0f, 0f, distanceZ.magnitude * _followSpeed);
			//_rigi.MovePosition(transform.position + newDir * Time.deltaTime);
			//****
			//_rigi.AddForce(direction * _followSpeed, ForceMode.Acceleration);
			
			//Vector3 strafe = (targetPos - transform.position).normalized;
			//_rigi.MovePosition(transform.position + strafe * _strafeSpeed * Time.deltaTime);
			
			//if(distanceX >= 0.5f)
			//	_rigi.AddForce((strafeRight ? Vector3.right : Vector3.left) * distanceX * _strafeSpeed, ForceMode.Acceleration);
			//****
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