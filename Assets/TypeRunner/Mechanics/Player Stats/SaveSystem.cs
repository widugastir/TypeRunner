using SoundSteppe.RefSystem;
using SoundSteppe.JsonSS;
using System.Collections;
using UnityEngine;

namespace TypeRunner
{
	public class SaveSystem : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private Settings _settings;
		
		public static event System.Action OnBeginSave;
		public static event System.Action OnEndSave;
		public static event System.Action OnBeginLoad;
		public static event System.Action OnEndLoad;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_settings = FindObjectOfType<Settings>(true);
			}
		}
		
		private IEnumerator Start()
		{
			yield return null;
			Load();
		}
		
		private void OnApplicationQuit()
		{
			Save();
		}
		
		private void OnApplicationFocus(bool focus)
		{
			if(focus == false)
				Save();
		}
		
		public void Save()
		{
			OnBeginSave?.Invoke();
			JsonSS.SaveGameObject("ldata", _stats);
			JsonSS.SaveGameObject("sdata", _settings);
			OnEndSave?.Invoke();
		}
		
		public void Load()
		{
			OnBeginLoad?.Invoke();
			JsonSS.LoadGameObject("ldata", _stats);
			JsonSS.LoadGameObject("sdata", _settings);
			OnEndLoad?.Invoke();
		}
	}
}