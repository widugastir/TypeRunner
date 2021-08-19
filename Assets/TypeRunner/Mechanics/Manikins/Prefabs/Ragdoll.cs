using System.Collections;
using System.Collections.Generic;
using SoundSteppe.RefSystem;
using UnityEngine;

public class Ragdoll : MonoBehaviour, INeedReference
{
	[SerializeField] public Rigidbody[] Rigidbodies;
	
	//------METHODS
	public void UpdateReferences(bool sceneObject) 
	{
		if(sceneObject == true)
		{
			//Rigidbodies = FindObjectsOfType<Rigidbody>(true);
		}
	}
	
	private void Start()
	{
		AddForce(new Vector3(0f, 100f, -60f), ForceMode.Impulse);
	}
	
	public void AddForce(Vector3 force, ForceMode mode)
	{
		for(int i = 0; i < Rigidbodies.Length; i++)
		{
			if(Rigidbodies[i].gameObject.activeSelf)
			Rigidbodies[Random.Range(0, Rigidbodies.Length)].AddForce(force, mode);
		}
	}
}
