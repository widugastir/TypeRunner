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
		private float _beginXPos;
		private float _targetXPos;
		
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
		}
		
		private void OnDisable()
		{
			_controlPanel.OnStartDrag -= OnStartDrag;
			_controlPanel.OnProcessDrag -= OnProcessDrag;
			_controlPanel.OnStopDrag -= OnStopDrag;
		}
		
		private void FixedUpdate()
		{
			MoveManikins();
		}
	    
		private void OnStartDrag()
		{
			_beginXPos = _manikins[0].transform.position.x;
		}
		
		private void OnStopDrag()
		{
			
		}
		
		private void OnProcessDrag(float delta)
		{
			
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