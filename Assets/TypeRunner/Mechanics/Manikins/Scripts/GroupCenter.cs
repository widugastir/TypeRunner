using UnityEngine;
using Cinemachine;

namespace TypeRunner
{
	public class GroupCenter : MonoBehaviour
	{
		[SerializeField] private float _strafeMultiplier = 1f;
		[HideInInspector] public Vector3 _startPosition;
		[HideInInspector] public float _beginStrafeX;
		public Transform _manikinsParent;
		public Transform _gameplayCamera;
		public Transform _target;
		public Transform _groupCenter;
		private float _lastStrafe = 0f;
		public bool CanMove { get; set; } = true;
		
		private Vector3 _basePos;
		
		private void Start()
		{
			_startPosition = transform.position;
			_basePos = _groupCenter.transform.localPosition;
		}
		
		public void BeginStrafe()
		{
			_beginStrafeX = _groupCenter.transform.position.x;
		}
		
		public void SetStrafePos(float strafe)
		{
			Vector3 newPosition = _groupCenter.transform.position;
			newPosition.x = _beginStrafeX + (strafe * _strafeMultiplier);
			_groupCenter.transform.position = newPosition;
		}
		
		public void EndStrafe(float strafe)
		{
			Vector3 newPosition = _groupCenter.transform.position;
			newPosition.x = _manikinsParent.position.x;
			_groupCenter.transform.position = newPosition;
			
			_beginStrafeX = 0f;
			_lastStrafe = strafe;
		}
		
		private void SmoothStrafe()
		{
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
		}
		
		public void Reset()
		{
			_groupCenter.transform.localPosition = _basePos;
			transform.position = _startPosition;
			//_upMove = false;
			CanMove = true;
		}
	}
}