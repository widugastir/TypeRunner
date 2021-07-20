using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
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
			var scripts = GameObject.FindObjectsOfType<Component>(true);
			UpdateScriptsRefs(scripts);
				
			scripts = Resources.FindObjectsOfTypeAll<Component>();
			UpdateScriptsRefs(scripts, false);
			
			AssetDatabase.Refresh();
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
			//Debug.Log("[ReferenceSystem] Scripts updated: " + scripts.Count());
			//Debug.Log("[ReferenceSystem] Prefab scripts updated: " + prefabs.Count());
		}
		 
		private static void UpdateScriptsRefs(Component[] scripts, bool sceneObject = true)
		{
			foreach(var s in scripts)
			{
				INeedReference inr = s as INeedReference;
				if(inr != null)
				{
					inr.UpdateReferences(sceneObject);
					if(sceneObject == false)
						EditorUtility.SetDirty(s);
				}
			}
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
				//UpdateReferences();
			}
		}
		
		/// <summary>
		/// Update references before build begin
		/// </summary>
		public int callbackOrder { get { return 0; } }
		public void OnPreprocessBuild(BuildReport report)
		{
			//UpdateReferences();
		}
	}
	#endif
	
	public interface INeedReference
	{
		void UpdateReferences(bool sceneObject);
	}
}