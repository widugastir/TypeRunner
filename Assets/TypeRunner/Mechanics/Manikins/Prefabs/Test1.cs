using UnityEngine;

public class Test1 : MonoBehaviour
{
	public float _speed = 1f;
	private Rigidbody _rigi;
	
    void Start()
    {
	    _rigi = GetComponent<Rigidbody>();
    }
    
	Vector3 force;
    
	void Update()
	{
		//force = ;
	}

	void FixedUpdate()
    {
	    _rigi.MovePosition(_rigi.position + Vector3.forward * _speed * Time.deltaTime);
    }
}
