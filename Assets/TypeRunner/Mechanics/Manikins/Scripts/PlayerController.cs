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
		[SerializeField, HideInInspector] private MapGeneration _generator;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private ControlPanel _controlPanel;
		[SerializeField, HideInInspector] private CinemachineTargetGroup _cameraTargetGroup;
		private bool _canMove = true;
		private int _manikinsCollected = 0;
		
		public bool IsMovementEnabled { get; private set; } = true;
	
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_levelManager = FindObjectOfType<LevelManager>();
				_generator = FindObjectOfType<MapGeneration>();
				_stats = FindObjectOfType<PlayerStats>();
				_groupCenter = gameObject.GetComponentInChildren<GroupCenter>();
				_controlPanel = gameObject.GetComponentInChildren<ControlPanel>();
				_cameraTargetGroup = gameObject.GetComponentInChildren<CinemachineTargetGroup>();
			}
		}
		
		private void OnEnable()
		{
			_controlPanel.OnStartDrag += OnStartDrag;
			_controlPanel.OnProcessDrag += OnProcessDrag;
			_controlPanel.OnStopDrag += OnStopDrag;
			ManikinMovement.OnBorderCollide += OnBorderCollide;
			Mankin.OnChangeOwner += OnChangeOwner;
			//SaveSystem.OnEndLoad += Init;
		}
		
		private void OnDisable()
		{
			_controlPanel.OnStartDrag -= OnStartDrag;
			_controlPanel.OnProcessDrag -= OnProcessDrag;
			_controlPanel.OnStopDrag -= OnStopDrag;
			ManikinMovement.OnBorderCollide -= OnBorderCollide;
			Mankin.OnChangeOwner -= OnChangeOwner;
			//SaveSystem.OnEndLoad -= Init;
		}
		
		private void FixedUpdate()
		{
			MoveManikins();
		}
		
		private void Start()
		{
			//Init();
		}
	    
		public void Init()
		{
			Vector3 pos = transform.position;
			for(int i = 0; i < _stats.StartUnitsLevel; i++)
			{
				pos = transform.position;
				pos += Vector3.forward * Random.Range(-2f, 2f) + Vector3.right * Random.Range(-2f, 2f);
				var man = _generator.SpawnManikin(pos);
				man.SetOwnerTo(false);
			}
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
					if(_manikins.Count == 0)
					{
						_levelManager.FinishLevel(false, _manikinsCollected);
					}
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
			manikin.SetFinished();
			_manikins.Remove(manikin);
			StopSpectateTo(manikin, false);
			
			_manikinsCollected++;
			if(_manikins.Count == 0)
			{
				_levelManager.FinishLevel(true, _manikinsCollected, manikin.EarnedCoinsBonus);
			}
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
				if(manikin != null)
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
				if(manikin != null)
					manikin.Movement.Move(transform.forward * _moveSpeed);
			}
		}
		
		public void MakeFormationLine()
		{
			float centerZ = _groupCenter.transform.position.z;
			float centerX = _groupCenter.transform.position.x;
			float offsetZ = 3f;
			float offsetX = .5f;
			
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].gameObject.layer = LayerMask.NameToLayer(_ignoreSameMask);
				_manikins[i].Rank = _manikins.Count - i;
				
				float x = centerX + offsetX * _manikins[i].Rank - (_manikins.Count - 1) * offsetX;
				float z = centerZ + offsetZ * i;
				Vector3 newPos = _manikins[i].transform.position;
				newPos.z = z;
				newPos.x = x;
				_manikins[i].transform.position = newPos;
			}
		}
		
		public void MakeFormationTriangle()
		{
			int rank = 1;
			int mans = 0;
			
			for(int i = 0; i < _manikins.Count; i++)
			{
				mans++;
				_manikins[i].Rank = rank;
				_manikins[i].RankPosition = mans;
				if(mans == rank && i != _manikins.Count - 1)
				{
					rank++;
					mans = 0;
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
		
		public void Reset()
		{
			_manikins.Clear();
			_manikinsCollected = 0;
			//Init();
		}
	}
}