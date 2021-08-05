using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class GameStarter : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private GameObject _previewCamera;
		[SerializeField, HideInInspector] private GroupCenter _groupCenter;
		[SerializeField, HideInInspector] private Canvas _canvas;
		[SerializeField, HideInInspector] private PlayerController _player;
		[SerializeField, HideInInspector] private LevelManager _levelManager;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_canvas = gameObject.GetComponentInChildren<Canvas>();
			if(sceneObject)
			{
				_player = FindObjectOfType<PlayerController>();
				_levelManager = FindObjectOfType<LevelManager>();
				_groupCenter = FindObjectOfType<GroupCenter>(true);
			}
		}
		
		private void Start()
		{
			Time.timeScale = 0f;
		}
		
		public void BeginPlay()
		{
			//_groupCenter.Reset();
			//_player.ResetMansParent();
			_previewCamera.SetActive(false);
			_levelManager.StartLevel();
			_canvas.gameObject.SetActive(false);
			Time.timeScale = 1f;
			_player.LateInitMans();
		}
		
	}
}