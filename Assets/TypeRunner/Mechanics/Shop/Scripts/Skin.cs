using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class Skin : MonoBehaviour, INeedReference
	{
		public SkinType _type;
		public Transform _hand;
		public GameObject _skinObject;
		public int MainMaterialIndex = 0;
		[SerializeField] public AnimationEvents _events;
		[SerializeField, HideInInspector] public Animator animator;
		[SerializeField, HideInInspector] public Renderer _renderer;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == false)
			{
				animator = GetComponentInChildren<Animator>(true);
				_renderer = GetComponentInChildren<Renderer>(true);
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