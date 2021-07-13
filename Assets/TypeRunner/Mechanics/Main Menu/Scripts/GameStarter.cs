using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class GameStarter : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Canvas _canvas;
		[SerializeField, HideInInspector] private PlayerController _player;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_canvas = gameObject.GetComponentInChildren<Canvas>();
			if(sceneObject)
				_player = FindObjectOfType<PlayerController>();
		}
		
		private void Start()
		{
			Time.timeScale = 0f;
		}
		
		public void BeginPlay()
		{
			_player.Init();
			_canvas.gameObject.SetActive(false);
			Time.timeScale = 1f;
		}
		
	}
}