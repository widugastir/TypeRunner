using UnityEngine;

public class MapMovement : MonoBehaviour
{
	[SerializeField] private float _moveSpeed = 1f;
	[SerializeField] private CameraController _camera;
	private float _baseSpeed;
	public bool CanMove { get; set; } = true;
	private Vector3 _basePosition;
	
	private void Start()
	{
		_basePosition = transform.position;
		_baseSpeed = _moveSpeed;
	}
	
	public void SetUpMovement(float speed)
	{
		SetSpeed(speed);
		_camera.SetUpMovement(1f);
	}
	
	public void SetSpeed(float speed)
	{
		_moveSpeed = speed;
	}
	
	private void FixedUpdate()
	{
		if(CanMove == false)
			return;
	    transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
	}
    
	public void Reset()
	{
		transform.position = _basePosition;
		_moveSpeed = _baseSpeed;
	}
}