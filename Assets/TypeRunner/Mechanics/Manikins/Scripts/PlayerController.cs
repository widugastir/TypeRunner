using System.Collections.Generic;
using SoundSteppe.RefSystem;
using NaughtyAttributes;
using Cinemachine;
using UnityEngine;

namespace TypeRunner
{
	public class PlayerController : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, Layer] private string _ignoreSameMask;
		[SerializeField] private float _moveSpeed = 1f;
		[SerializeField] private List<Mankin> _manikins;
		[SerializeField, HideInInspector] private GroupCenter _groupCenter;
		[SerializeField, HideInInspector] private ControlPanel _controlPanel;
		[SerializeField, HideInInspector] private CinemachineTargetGroup _cameraTargetGroup;
		private bool _canMove = true;
		
		public bool IsMovementEnabled { get; private set; } = true;
	
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_groupCenter = gameObject.GetComponentInChildren<GroupCenter>();
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
		
		public void SendCommand(ManikinCommands.E_Command command)
		{
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].Commands.DoCommand(command);
			}
		}
		
		private void OnChangeOwner(Mankin manikin, bool isNeutral)
		{
			if(isNeutral)
			{
				if(_manikins.Contains(manikin))
				{
					_manikins.Remove(manikin);
					StopSpectateTo(manikin);
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
		
		public void ManikinFinished(Mankin manikin)
		{
			manikin.Movement.SetCanMove(false);
			manikin.SetOwnerTo(true);
		}
		
		public void StopSpectateTo(Mankin manikin, bool minOne = true)
		{
			if(minOne && _cameraTargetGroup.m_Targets.Length <= 1)
				return;
			_cameraTargetGroup.RemoveMember(manikin.transform);
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
		
		public void MakeFormation()
		{
			int rank = 1;
			int mans = 0;
			
			for(int i = 0; i < _manikins.Count; i++)
			{
				mans++;
				_manikins[i].Rank = rank;
				_manikins[i].RankPosition = mans;
				//print(_manikins[i].RankPosition);
				if(mans == rank && i != _manikins.Count - 1)
				{
					rank++;
					mans = 0;
					//print(rank);
				}
			}
			
			int centerRank = rank / 2;
			float centerZ = _groupCenter.transform.position.z;
			float centerX = _groupCenter.transform.position.x;
			float rankStepY = 3f;
			float rankStepX = 1f;
			float centerRankX = 0f;
			
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].gameObject.layer = LayerMask.NameToLayer(_ignoreSameMask);
				centerRankX = (float)_manikins[i].Rank / 2f;
				
				float z = centerZ + ((float)_manikins[i].Rank - (float)centerRank) * rankStepY;
				float x = centerX + ((float)_manikins[i].RankPosition - (float)centerRankX) * rankStepX;
				Vector3 newPos = _manikins[i].transform.position;
				newPos.z = z;
				newPos.x = x;
				_manikins[i].transform.position = newPos;
				
				// Sort by ranks
				_manikins[i].Rank = rank - _manikins[i].Rank + 1;
			}
		}
	}
}