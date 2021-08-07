using UnityEngine;
using Cinemachine;

namespace TypeRunner
{
	public class GroupCenter : MonoBehaviour
	{
		[SerializeField] private float _speed = 1f;
		[SerializeField] private float _strafeMultiplier = 1f;
		public Transform _gameplayCamera;
		public Transform _target;
		public Transform _groupCenter;
		[HideInInspector] public Vector3 _startPosition;
		private float _targetStrafe = 0f;
		private bool _upMove = false;
		public bool CanMove { get; set; } = true;
		
		private float _currentStrafe = 0f;
		private Vector3 _basePos;
		
		private void Start()
		{
			_startPosition = transform.position;
			_basePos = _groupCenter.transform.localPosition;
		}
		
		public void SetStrafePos(float strafe)
		{
			_targetStrafe = strafe;
			Vector3 newPosition = _groupCenter.transform.position;
			newPosition.x = strafe * _strafeMultiplier;
			_groupCenter.transform.position = newPosition;
		}
		
		public void EndStrafe()
		{
			//print(123);
			//_basePos = _groupCenter.transform.position;
		}
		
		private void Update()
		{
			//SmoothStrafe();
		}
		
		private void SmoothStrafe()
		{
			//float _lerpSpeed = 1f;
			//float _strafe = Mathf.Lerp(_currentStrafe, _targetStrafe, Time.deltaTime * _lerpSpeed);
			
			//Vector3 newPosition = _groupCenter.transform.position;
			//newPosition.x = _strafe * _strafeMultiplier;
			//_groupCenter.transform.position = newPosition;
			
			//_currentStrafe = _strafe;
		}
		
		public void Move()
		{
			if(CanMove == false)
				return;
				
			Vector3 newPos = transform.position;
			newPos.x = _gameplayCamera.position.x;
			newPos.y = _target.position.y;
			newPos.z = _target.position.z;
			transform.position = newPos;
		}
		
		public void SetUpMovement()
		{
			_upMove = true;
		}
		
		public void Reset()
		{
			_groupCenter.transform.localPosition = _basePos;
			transform.position = _startPosition;
			_upMove = false;
			CanMove = true;
		}
	}
}