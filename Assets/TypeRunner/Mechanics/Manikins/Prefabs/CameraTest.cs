using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
	public Transform target;
	private Vector3 offset;
	
	void OnEnable()
    {
	    offset = transform.position - target.position;
    }

	void LateUpdate()
    {
	    transform.position = target.position + offset;
    }
}
