using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace TypeRunner
{
	public class PlayerController : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private float _moveSpeed = 1f;
		[SerializeField] private List<ManikinMovement> _manikins;
		[SerializeField, HideInInspector] private ControlPanel _controlPanel;
		private bool _canMove = true;
		
		public bool IsMovementEnabled { get; private set; } = true;
	
		//------METHODS
		[Button]
		private void UpdateReferences()
		{
			_controlPanel = gameObject.GetComponentInChildren<ControlPanel>();
		}
		
		private void OnEnable()
		{
			_controlPanel.OnStartDrag += OnStartDrag;
			_controlPanel.OnProcessDrag += OnProcessDrag;
			_controlPanel.OnStopDrag += OnStopDrag;
			ManikinMovement.OnBorderCollide += OnBorderCollide;
		}
		
		private void OnDisable()
		{
			_controlPanel.OnStartDrag -= OnStartDrag;
			_controlPanel.OnProcessDrag -= OnProcessDrag;
			_controlPanel.OnStopDrag -= OnStopDrag;
			ManikinMovement.OnBorderCollide -= OnBorderCollide;
		}
		
		private void FixedUpdate()
		{
			MoveManikins();
		}
	    
		private void OnStartDrag()
		{
			_canMove = true;
			foreach(var manikin in _manikins)
			{
				manikin.InitStrafe();
			}
		}
		
		private void OnProcessDrag(float delta)
		{
			if(_canMove)
				StrafeMankins(delta);
		}
		
		private void OnStopDrag()
		{
			StopStrafeMankins();
		}
		
		private void OnBorderCollide(Vector3 point)
		{
			StopStrafeMankins();
			PushMankinsAway(point);
		}
		
		private void PushMankinsAway(Vector3 point)
		{
			foreach(var manikin in _manikins)
			{
				manikin.PushAway(point, 0.1f);
			}
		}
		
		private void StopStrafeMankins()
		{
			_canMove = false;
			foreach(var manikin in _manikins)
			{
				manikin.StopStrafe();
			}
		}
		
		private void StrafeMankins(float delta)
		{
			if(IsMovementEnabled == false)
				return;
		    	
			foreach(var manikin in _manikins)
			{
				manikin.Strafe(delta);
			}
		}
	    
		private void MoveManikins()
		{
			if(IsMovementEnabled == false)
				return;
		    	
			foreach(var manikin in _manikins)
			{
				manikin.Move(transform.forward * _moveSpeed);
			}
		}
	}
}