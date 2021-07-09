using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
#endif

namespace SoundSteppe.RefSystem 
{
	#if UNITY_EDITOR
	[InitializeOnLoadAttribute]
	public class ReferencesSystem : IPreprocessBuildWithReport
	{
		[MenuItem("Tools/Update references")]
		private static void UpdateReferences() 
		{
			var scripts = GameObject.FindObjectsOfType<Component>(true).OfType<INeedReference>();
			foreach(var s in scripts)
				s.UpdateReferences();
				
			var prefabs = Resources.FindObjectsOfTypeAll<Component>().OfType<INeedReference>();
			foreach(var p in prefabs)
				p.UpdateReferences();
				
			//Debug.Log("[ReferenceSystem] Scripts updated: " + scripts.Count());
			//Debug.Log("[ReferenceSystem] Prefab scripts updated: " + prefabs.Count());
		}
		 
		 
		/// <summary>
		/// Update references on script compiling
		/// </summary>
		static ReferencesSystem()
		{
			EditorApplication.playModeStateChanged += playModeStatChange;
			//UpdateReferences();
		} 
		
		/// <summary>
		/// Update references on enter play mode
		/// </summary>
		private static void playModeStatChange(PlayModeStateChange state)
		{
			if(state == PlayModeStateChange.ExitingEditMode)
			{
				UpdateReferences();
			}
		}
		
		/// <summary>
		/// Update references before build begin
		/// </summary>
		public int callbackOrder { get { return 0; } }
		public void OnPreprocessBuild(BuildReport report)
		{
			UpdateReferences();
		}
	}
	#endif
	
	public interface INeedReference
	{
		void UpdateReferences();
	}
}