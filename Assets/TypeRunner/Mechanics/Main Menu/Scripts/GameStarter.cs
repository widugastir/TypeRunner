using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class GameStarter : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private Canvas _canvas;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			_canvas = gameObject.GetComponentInChildren<Canvas>();
		}
		
		private void Start()
		{
			Time.timeScale = 0f;
		}
		
		public void BeginPlay()
		{
			_canvas.gameObject.SetActive(false);
			Time.timeScale = 1f;
		}
		
	}
}