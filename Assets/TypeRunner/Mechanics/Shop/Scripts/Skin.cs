using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Skin : MonoBehaviour, INeedReference
	{
		public SkinType _type;
		public GameObject _skinObject;
		[SerializeField, HideInInspector] public Animator animator;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == false)
			{
				animator = gameObject.GetComponentInChildren<Animator>();
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