using System.Collections.Generic;
using SoundSteppe.RefSystem;
using Cinemachine;
using UnityEngine;

namespace TypeRunner
{
	public class PlayerController : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private float _moveSpeed = 1f;
		[SerializeField] private List<Mankin> _manikins;
		[SerializeField, HideInInspector] private ControlPanel _controlPanel;
		[SerializeField, HideInInspector] private CinemachineTargetGroup _cameraTargetGroup;
		private bool _canMove = true;
		
		public bool IsMovementEnabled { get; private set; } = true;
	
		//------METHODS
		public void UpdateReferences()
		{
			_controlPanel = gameObject.GetComponentInChildren<ControlPanel>();
			_cameraTargetGroup = gameObject.GetComponentInChildren<CinemachineTargetGroup>();
		}
		
		private void OnEnable()
		{
			_controlPanel.OnStartDrag += OnStartDrag;
			_controlPanel.OnProcessDrag += OnProcessDrag;
			_controlPanel.OnStopDrag += OnStopDrag;
			ManikinMovement.OnBorderCollide += OnBorderCollide;
			Mankin.OnChangeOwner += OnChangeOwner;
		}
		
		private void OnDisable()
		{
			_controlPanel.OnStartDrag -= OnStartDrag;
			_controlPanel.OnProcessDrag -= OnProcessDrag;
			_controlPanel.OnStopDrag -= OnStopDrag;
			ManikinMovement.OnBorderCollide -= OnBorderCollide;
			Mankin.OnChangeOwner -= OnChangeOwner;
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
				manikin.Movement.InitStrafe();
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
		
		public void BlockManikins(int amount)
		{
			for(int i = 0; i < amount && i < _manikins.Count; i++)
			{
				_manikins[i].Commands.BlockCommand();
			}
		}
		
		private void OnChangeOwner(Mankin manikin, bool isNeutral)
		{
			if(isNeutral)
			{
				if(_manikins.Contains(manikin))
				{
					_manikins.Remove(manikin);
					_cameraTargetGroup.RemoveMember(manikin.transform);
				}
			}
			else
			{
				if(!_manikins.Contains(manikin))
				{
					_manikins.Add(manikin);
					_cameraTargetGroup.AddMember(manikin.transform, 1f, 0f);
				}
			}
		}
		
		private void PushMankinsAway(Vector3 point)
		{
			foreach(var manikin in _manikins)
			{
				manikin.Movement.PushAway(point, 0.1f);
			}
		}
		
		private void StopStrafeMankins()
		{
			_canMove = false;
			foreach(var manikin in _manikins)
			{
				manikin.Movement.StopStrafe();
			}
		}
		
		private void StrafeMankins(float delta)
		{
			if(IsMovementEnabled == false)
				return;
		    	
			foreach(var manikin in _manikins)
			{
				manikin.Movement.Strafe(delta);
			}
		}
	    
		private void MoveManikins()
		{
			if(IsMovementEnabled == false)
				return;
		    	
			foreach(var manikin in _manikins)
			{
				manikin.Movement.Move(transform.forward * _moveSpeed);
			}
		}
	}
}