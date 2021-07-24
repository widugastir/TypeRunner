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
		private float _strafe = 0f;
		private bool _upMove = false;
		public bool CanMove { get; set; } = true;
		
		private void Start()
		{
			_startPosition = transform.position;
		}
		
		public void SetStrafePos(float strafe)
		{
			_strafe = strafe;
			//Vector3 newPosition = transform.position;
			Vector3 newPosition = _groupCenter.transform.position;
			newPosition.x = strafe * _strafeMultiplier;
			//transform.position = newPosition;
			_groupCenter.transform.position = newPosition;
		}
		
		public void Move()
		{
			if(CanMove == false)
				return;
			//Vector3 direction = Vector3.forward * _speed;
			//if(_upMove)
			//{
			//	direction = Vector3.forward * _speed / 4f + Vector3.up * _speed / 4f;
			//}
			Vector3 newPos = transform.position;
			newPos.x = _gameplayCamera.position.x;
			newPos.y = _target.position.y;
			newPos.z = _target.position.z;
			transform.position = newPos;
			//transform.Translate(direction * Time.deltaTime);
			//_rigi.AddForce(direction, ForceMode.Acceleration);
		}
		
		public void SetUpMovement()
		{
			_upMove = true;
		}
		
		public void Reset()
		{
			transform.position = _startPosition;
			_upMove = false;
			CanMove = true;
		}
	}
}