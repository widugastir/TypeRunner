using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Skin : MonoBehaviour, INeedReference
	{
		public SkinType _type;
		public GameObject _skinObject;
		public int MainMaterialIndex = 0;
		[SerializeField, HideInInspector] public Animator animator;
		[SerializeField, HideInInspector] public Renderer renderer;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == false)
			{
				animator = GetComponentInChildren<Animator>(true);
				renderer = GetComponentInChildren<Renderer>(true);
			}
		}
	}
	
	public enum SkinType
	{
		S1,
		S2,
		S3,
		S4,
		S5,
		S6,
		S7,
		S8,
		S9
	}
}