using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private bool _moveUp = false;
	private float _speed;
	
	public void SetUpMovement(float speed)
	{
		_speed = speed;
		_moveUp = true;
	}
	
	public void Update()
    {
	    if(_moveUp)
	    {
	    	//transform.Translate(Vector3.up * _speed * Time.deltaTime);
	    }
    }
    
	public void Reset()
	{
		_moveUp = false;
	}
}
