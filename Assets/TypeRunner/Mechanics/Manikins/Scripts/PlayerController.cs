using System.Collections.Generic;
using System.Collections;
using SoundSteppe.RefSystem;
using NaughtyAttributes;
using Cinemachine;
using UnityEngine;

namespace TypeRunner
{
	public class PlayerController : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private ImmortalTimer _immortalTimer;
		[SerializeField] private Transform _manikinsParent;
		[SerializeField] private float _finishDelay = 2f;
		[SerializeField, Layer] private string _ignoreSameMask;
		[SerializeField] private List<Mankin> _manikins;
		[SerializeField, HideInInspector] public MapMovement _mapMovement;
		[SerializeField, HideInInspector] private GroupCenter _groupCenter;
		[SerializeField, HideInInspector] private MapGenerationLevels _generator;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		[SerializeField, HideInInspector] private ControlPanel _controlPanel;
		[SerializeField, HideInInspector] private CinemachineTargetGroup _cameraTargetGroup;
		private bool _canMove = true;
		private bool _controllable = true;
		private int _manikinsCollected = 0;
		private Ladder _lastLadder;
		private Vector3 _baseParentPos;
		public bool IsMovementEnabled { get; set; } = true;
		private readonly Vector3 _forward = Vector3.forward;
		private readonly Vector3 _right = Vector3.right;
	
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_mapMovement = FindObjectOfType<MapMovement>();
				_levelManager = FindObjectOfType<LevelManager>();
				_generator = FindObjectOfType<MapGenerationLevels>();
				_stats = FindObjectOfType<PlayerStats>();
				_groupCenter = gameObject.GetComponentInChildren<GroupCenter>();
				_controlPanel = gameObject.GetComponentInChildren<ControlPanel>();
				_cameraTargetGroup = gameObject.GetComponentInChildren<CinemachineTargetGroup>();
			}
		}
		
		private void OnEnable()
		{
			ManikinMovement.OnBorderCollide += OnBorderCollide;
			Mankin.OnChangeOwner += OnChangeOwner;
			PlayerStats.OnStartUnitChange += OnStartUnitChange;
			ObstacleZone.EnterZone += PlayerEnterZone;
		}
		
		private void OnDisable()
		{
			ManikinMovement.OnBorderCollide -= OnBorderCollide;
			Mankin.OnChangeOwner -= OnChangeOwner;
			PlayerStats.OnStartUnitChange -= OnStartUnitChange;
			ObstacleZone.EnterZone -= PlayerEnterZone;
		}
		
		protected void LateUpdate()
		{
			MoveGroupCenter();
		}
		
		protected void Update()
		{
			//if(Input.GetKeyDown(KeyCode.K))
			//	_levelManager.FinishLevel();
				
			//if(Input.GetKeyDown(KeyCode.L))
			//	_levelManager.FinishLevel();
		}
		
		private void PlayerEnterZone(ObstacleZone zone)
		{
			if(zone._disableStrafe)
			{
				_controllable = false;
			}
			else
			{
				_controllable = true;
			}
		}
		
		private void Start()
		{
			_baseParentPos = _manikinsParent.position;
		}
		
		public void SetMove(bool canMove)
		{
			_mapMovement.CanMove = canMove;
		}
	    
		private void OnStartUnitChange(int amount)
		{
			Init();
		}
	    
		public void Init(bool makeImmortal = false, float immortalTime = 0f)
		{
			for(int i = 0; i < _manikins.Count; i++)
			{
				Destroy(_manikins[i].gameObject);
			}
			_manikins.Clear();
			
			Vector3 pos = transform.position;
			
			if(makeImmortal && immortalTime > 0f)
				_immortalTimer.Enable(immortalTime);
			
			for(int i = 0; i < _stats.StartUnitsLevel; i++)
			{
				pos = transform.position;
				pos += Vector3.forward * Random.Range(-2f, 2f) + Vector3.right * Random.Range(-2f, 2f);
				var man = _generator.SpawnManikin(pos);
				man.SetIdle(true);
				man.SetOwnerTo(false);
				if(makeImmortal)
				{
					man.SetImmortal(true, immortalTime, true);
				}
			}
		}
		
		public void MultiplyStikmans(float multiply = 2f)
		{
			int totalStickmans = Mathf.RoundToInt((float)_manikins.Count * multiply);
			int stickmanToSplit = totalStickmans - _manikins.Count;
			for(int i = 0; i < _manikins.Count; i++)
			{
				if(i >= stickmanToSplit)
					break;
				_manikins[i].Double();
			}
		}
		
		public void SetImmortal(float duration, bool affectZones)
		{
			_immortalTimer.Enable(duration);
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].SetImmortal(true, duration, affectZones);
			}
		}
		
		public void InitMans()
		{
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].SetIdle(false);
			}
		}
		
		public void LateInitMans()
		{
			StartCoroutine(InitM());
		}
		
		private IEnumerator InitM()
		{
			yield return null;
			InitMans();
		}
		
		public void SetMansSuccesfull(bool successful)
		{
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].IsWordSuccessful = successful;
			}
		}
		
		public void SetIndependentMove(bool independent)
		{
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].Movement.IsIndependetMovement = independent;
			}
		}
		
		private void MoveGroupCenter()
		{
			if(IsMovementEnabled == false)
				return;
			_groupCenter.Move();
			
			if(_controllable == false)
				return;
			Vector3 newPos = _manikinsParent.position;
			newPos.x = _groupCenter._groupCenter.position.x;
			_manikinsParent.position 
				= Vector3.SmoothDamp(_manikinsParent.position, 
				newPos, 
				ref velocity,
				0.03f);
		}
		
		private Vector3 velocity = Vector3.zero;
		private void StrafeGroupCenter(float strafe)
		{
			if(_controllable == false)
				return;
				
			_groupCenter.SetStrafePos(strafe);
			
			//Vector3 newPos = _manikinsParent.position;
			//newPos.x = _groupCenter._groupCenter.position.x;
			//_manikinsParent.position = newPos;
		}
	    
		public void OnStartDrag(float startPos)
		{
			_canMove = true;
		}
		
		public void OnProcessDrag(float _prevPos, float delta)
		{
			if(_canMove && IsMovementEnabled)
			{
				StrafeGroupCenter(delta);
				foreach(var manikin in _manikins)
				{
					if(Mathf.Abs(delta - _prevPos) <= 0.01f)
						manikin.Movement.SerDirection(_forward);
					else
					{
						manikin.Movement.SerDirection(_forward + _right * 1f * Mathf.Sign(delta - _prevPos));
					}
				}
			}
		}
		
		public void OnEndDrag()
		{
			_groupCenter.EndStrafe();
			foreach(var manikin in _manikins)
			{
				manikin.Movement.SerDirection(_forward);
			}
		}
		
		private void OnBorderCollide(Vector3 point)
		{
		}
		
		public void BlockManikins(int amount)
		{
			for(int i = 0; i < amount && i < _manikins.Count; i++)
			{
				_manikins[i].Commands.SetBlock(true);
				_manikins[i].BlockStickman(_ignoreSameMask, true);
				_manikins[i].IsWordSuccessful = false;
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
						_levelManager.PreFinishLevel(false, _manikinsCollected);
					}
				}
			}
			else
			{
				if(!_manikins.Contains(manikin))
				{
					manikin.Movement.Init(_groupCenter._groupCenter);
					manikin.transform.SetParent(_manikinsParent);
					_manikins.Add(manikin);
					_cameraTargetGroup.AddMember(manikin.transform, 1f, 0f);
				}
			}
		}
		
		public void ManikinFinished(Mankin manikin, Ladder ladder)
		{
			_lastLadder = ladder;
			_mapMovement.SetUpMovement(5f);
			
			manikin.Movement.MoveToGroupCenter = false;
			manikin.transform.SetParent(_mapMovement.transform);
			manikin.Movement.SetCanMove(false);
			manikin.SetFinished();
			StopSpectateTo(manikin, false);
			
			_manikinsCollected++;
			if(_manikins.Count == 1)
			{
				_mapMovement.CanMove = false;
				_groupCenter.CanMove = false;
				StartCoroutine(FinishLevel(manikin.EarnedCoinsBonus));
				_lastLadder.PlayFinishParticles(manikin.transform.position);
			}
			_manikins.Remove(manikin);
		}
		
		private IEnumerator FinishLevel(float coinBonus)
		{
			yield return new WaitForSecondsRealtime(_finishDelay);
			_levelManager.PreFinishLevel(true, _manikinsCollected, coinBonus);
			_lastLadder.StopFinishParticles();
		}
		
		public void StopSpectateTo(Mankin manikin, bool minOne = true)
		{
			if(minOne && _cameraTargetGroup.m_Targets.Length <= 1)
				return;
			_cameraTargetGroup.RemoveMember(manikin.transform);
		}
		
		public void SpactateOnlyFor(Mankin manikin)
		{
			_cameraTargetGroup.m_Targets = null;
			_cameraTargetGroup.AddMember(manikin.transform, 1f, 0f);
		}
		
		public void SetControllable(bool controllable)
		{
			_controllable = controllable;
		}
		
		public void MakeFormationLine()
		{
			float centerZ = _groupCenter.transform.position.z;
			float centerX = _groupCenter.transform.position.x;
			float offsetZ = 1f;
			float offsetX = 0f;
			
			print(_manikins.Count);
			
			for(int i = 0; i < _manikins.Count; i++)
			{
				_manikins[i].gameObject.layer = LayerMask.NameToLayer(_ignoreSameMask);
				_manikins[i].Rank = _manikins.Count - i;
				
				float x = centerX + offsetX * _manikins[i].Rank - (_manikins.Count - 1) * offsetX;
				float z = centerZ + offsetZ * i;
				Vector3 newPos = _manikins[i].transform.position;
				newPos.z = z;
				newPos.x = x;
				//_manikins[i].transform.position = newPos;
				_manikins[i].Movement.GoTo(newPos, 0.7f);
				if(i == 0)
				{
					SpactateOnlyFor(_manikins[i]);
				}
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
		
		public void ResetMansParent()
		{
			_manikinsParent.position = _baseParentPos;
		}
		
		public void Reset()
		{
			ResetMansParent();
			_groupCenter.Reset();
			_manikins.Clear();
			_manikinsCollected = 0;
			_mapMovement.Reset();
			_mapMovement.CanMove = true;
			_controllable = true;
		}
	}
}