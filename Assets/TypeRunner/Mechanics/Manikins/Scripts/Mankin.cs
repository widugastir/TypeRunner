using SoundSteppe.RefSystem;
using System.Collections;
using UnityEngine;

namespace TypeRunner
{
	public class Mankin : MonoBehaviour, INeedReference
	{
		//------FIELDS
		public bool IsNeutral = true;
		[HideInInspector] public ManikinMovement Movement;
		[HideInInspector] public ManikinCommands Commands;
		private bool IsIdle { get; set; } = false;
		public bool AffectZones = true;
		private bool _immortal = false;
		public bool Immortal 
		{ 
			get 
			{
				return _immortal; 
			} 
			set 
			{
				if(_blockImmortal == false)
					_immortal = value; 
			} 
		}
		private bool _blockImmortal = false;
		private MapGenerationLevels _map;
		
		public bool IsFinished { get; set; } = false;
		public bool IsWordSuccessful = false;
		public int RankPosition { get; set; } = 0;
		public int Rank { get; set; } = 0;
		public float EarnedCoinsBonus { get; set; } = 0f;
		[SerializeField] private ParticleSystem[] _dieVfx;
		[SerializeField] private BlinkingMaterial _blinking;
		
		public static event System.Action<Mankin, bool, bool> OnChangeOwner;
		[SerializeField, HideInInspector] public Animator _animator;
		[SerializeField, HideInInspector] public SkinChanger _skinChanger;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)  
		{
			if(sceneObject == false)
			{
				Movement = gameObject.GetComponentInChildren<ManikinMovement>();
				Commands = gameObject.GetComponentInChildren<ManikinCommands>();
				_animator = gameObject.GetComponentInChildren<Animator>();
				_skinChanger = gameObject.GetComponentInChildren<SkinChanger>();
			}
		}
		
		public void Init(MapGenerationLevels map)
		{
			_map = map;
			Commands.Init(map._player);
		}
		
		private void OnEnable()
		{
			Shop.OnSkinSelect += OnSkinSelect;
		}
		
		private void OnDisable()
		{
			Shop.OnSkinSelect -= OnSkinSelect;
			Immortal = false;
		}
		
		private void OnSkinSelect()
		{
			_animator = _skinChanger._current.animator;
		}
		
		public void SetImmortal(bool immortal, float immortalTime, bool affectZones)
		{
			//SetMortal(0f);
			AffectZones = affectZones;
			Immortal = immortal;
			if(immortal == true)
			{
				StartCoroutine(SetMortal(immortalTime));
				//SetFlickering(true, immortalTime);
				_blockImmortal = true;
			}
		}
		
		private IEnumerator SetMortal(float immortalTime)
		{
			yield return new WaitForSecondsRealtime(immortalTime);
			_blockImmortal = false;
			AffectZones = true;
			Immortal = false;
			//SetFlickering(false);
		}
		
		public void SetOwnerTo(bool neutral, bool instaKilled = false)
		{
			IsNeutral = neutral;
			OnChangeOwner?.Invoke(this, IsNeutral, instaKilled);
			_animator = _skinChanger._current.animator;
			if(neutral == false && IsIdle == false)
			{
				_animator.SetTrigger("Running");
			}
		}
		
		public void SetIdle(bool idle)
		{
			_animator = _skinChanger._current.animator;
			IsIdle = idle;
			if(IsIdle)
			{
				//_animator.SetTrigger("Idle");
			}
			else
			{
				_animator.SetTrigger("Running");
			}
		}
		
		public void SetFinished()
		{
			transform.Rotate(new Vector3(0f, 180f, 0f));
			_animator.SetTrigger("Dance");
			IsFinished = true;
		}
		
		private void OnCollisionEnter(Collision collisionInfo)
		{
			if(collisionInfo.gameObject.TryGetComponent(out Mankin mankin))
			{
				if(this.IsNeutral == false && mankin.IsNeutral)
					mankin.SetOwnerTo(false);
			}
			
			if(collisionInfo.gameObject.TryGetComponent(out Obstacle obstacle))
			{
				Kill();
			}
		}
		
		public void BlockStickman(string layer, bool moveForward = false)
		{
			gameObject.layer = LayerMask.NameToLayer(layer);
			Movement.MoveToGroupCenter = false;
			transform.SetParent(_map._levelParent);
			_immortal = false;
			if(moveForward)
			{
				Movement.SetCanMove2(true);
			}
		}
		
		public void SetFlickering(bool flicker, float duration = -1f)
		{
			if(flicker)
			{
				if(duration > 0f)
					_blinking.SetDuration(duration, 0.25f, 0.05f);
				_blinking.Enable();
				//_skinChanger.StartFlickering();
			}
			else
			{
				_blinking.Disable();
				//_skinChanger.StopFlickering();
			}
		}
		
		public void Double()
		{
			Mankin m = _map.SpawnManikin(transform.position + Vector3.forward * 1f);
			if(m != null)
			{
				m.SetOwnerTo(false);
				StartCoroutine(LateSetOwner(m));
			}
		}
		
		private IEnumerator LateSetOwner(Mankin m)
		{
			yield return null;
			m.SetOwnerTo(false);
		}
		
		public void Kill(bool instaKill = false)
		{
			if(Immortal && instaKill == false)
				return;
			if(instaKill == false)
			{
				foreach(var p in _dieVfx)
				{
					p.transform.SetParent(null);
					p.Play();
				}
				_map.SpawnRagdoll(transform.position, new Vector3(0f, 5000f, 0f));
			}
			Destroy(gameObject);
		}
		
		protected void OnDestroy()
		{
			SetOwnerTo(true, true);
		}
	}
}