using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTween : MonoBehaviour
{
	[SerializeField] private Vector3 _rotateSpeed;
	
	private void Update()
    {
	    transform.Rotate(_rotateSpeed * Time.unscaledDeltaTime);
    }
}
