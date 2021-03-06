using SoundSteppe.RefSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace TypeRunner
{
	public class ManikinMovement : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Rigidbody _rigi;
		[SerializeField] private Vector2 _leftRightBorders;
		[SerializeField] private float _strafeSpeed = 3f;
		[SerializeField] private float _forwardSpeed = 3f;
		[SerializeField] private float _flyHeight = 3f;
		[SerializeField] private float _flyDuration = 3f;
		[SerializeField] private float _climbHeight = 3f;
		private bool _canMove = true;
		public bool IsIndependetMovement { get; set; } = false;
		public bool MoveToGroupCenter { get; set; } = true;
		private bool CanMoveForward{ get; set; } = false;
		private Tween _goToTween;
		private Transform _groupCenter;
		private TweenCallback _hoverEndCallback;
		private Vector3 _targetDirection;
		private readonly Vector3 _empty = Vector3.zero;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_rigi = gameObject.GetComponentInChildren<Rigidbody>();
		}
		 
		public void Init(Transform groupCenter)
		{
			_groupCenter = groupCenter;
			_targetDirection = transform.rotation.eulerAngles;
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
		
		private void StrafeToCenter()
		{
			
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
			_rigi.AddForce(Vector3.forward * _forwardSpeed, ForceMode.VelocityChange);
		}
		
		private void FixedUpdate()
		{
			MoveToPoint();
			MoveForward();
			UpdateDirection();
			ClampInsideBorders();
		}
		
		private void ClampInsideBorders()
		{
			Vector3 newPos = transform.position;
			if(transform.position.x < _leftRightBorders.x)
			{
				newPos.x = _leftRightBorders.x;
				transform.position = newPos;
			}
			newPos = transform.position;
			if(transform.position.x > _leftRightBorders.y)
			{
				newPos.x = _leftRightBorders.y;
				transform.position = newPos;
			}
		}
		
		public void SerDirection(Vector3 direction)
		{
			_targetDirection = direction;
		}
		
		private void UpdateDirection()
		{
			if(MoveToGroupCenter == false || _targetDirection == _empty)
				return;
			Quaternion newRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_targetDirection), Time.deltaTime * 8f);
			transform.rotation = newRotation;
		}
		
		private void OnJumpEnd() 
		{ 
			_rigi.useGravity = true;
		}
		
		public void Jump(float height, float time)
		{
			_rigi.useGravity = false; 
			transform.DOLocalJump(transform.localPosition, height, 1, time)
				.SetEase(Ease.Linear)
				.OnComplete(OnJumpEnd);
		}
		
		public void Flying(TweenCallback flyEndCallback, TweenCallback hoverEndCallback)
		{
			_rigi.useGravity = false;
			_hoverEndCallback = hoverEndCallback;
			transform.DOMoveY(transform.position.y + _flyHeight, 1f)
				.SetEase(Ease.Linear)
				.OnComplete(flyEndCallback)
				.OnComplete(Hovering);
		}
		
		private void Hovering()
		{
			StartCoroutine(StopHovering());
			_rigi.useGravity = false;
		}
		
		private IEnumerator StopHovering()
		{
			yield return new WaitForSecondsRealtime(_flyDuration);
			_rigi.useGravity = true;
			_hoverEndCallback?.Invoke();
		}
		
		public void Climb()
		{
			_rigi.useGravity = false;
			_rigi.DOMoveY(transform.position.y + _climbHeight, 1f)
				.SetEase(Ease.Linear)
				.OnComplete(OnClimbEnd);
		}
		
		private void OnClimbEnd()
		{
			_rigi.useGravity = true;
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
		
		public void SetPhysics(bool gravity)
		{
			_rigi.useGravity = gravity;
		}
	}
}