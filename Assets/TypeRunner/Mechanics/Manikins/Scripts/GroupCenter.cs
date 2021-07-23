﻿using UnityEngine;
using Cinemachine;

namespace TypeRunner
{
	public class GroupCenter : MonoBehaviour
	{
		[SerializeField] private float _speed = 1f;
		[SerializeField] private float _strafeMultiplier = 1f;
		public Transform _groupCenter;
		public Vector3 _startPosition;
		private float _strafe = 0f;
		
		private void Start()
		{
			_startPosition = transform.position;
		}
		
		public void SetStrafePos(float strafe)
		{
			_strafe = strafe;
			Vector3 newPosition = _groupCenter.transform.position;
			newPosition.x = strafe * _strafeMultiplier;
			_groupCenter.transform.position = newPosition;
		}
		
		public void Move()
		{
			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
		}
		
		public void Reset()
		{
			transform.position = _startPosition;
		}
	}
}