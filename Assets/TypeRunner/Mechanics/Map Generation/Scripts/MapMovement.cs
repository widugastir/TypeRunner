using UnityEngine;

public class MapMovement : MonoBehaviour
{
	[SerializeField] public float _moveSpeed = 1f;
	[SerializeField] public float _pushBackDistance = 10f;
	//[SerializeField] private CameraController _camera;
	private float _baseSpeed;
	public bool CanMove { get; set; } = true;
	private Vector3 _basePosition;
	private Vector3 _lastSavePoint;
	
	private void Start()
	{
		_basePosition = transform.position;
		_baseSpeed = _moveSpeed;
		_lastSavePoint = transform.position;
	}
	
	public void SetUpMovement(float speed)
	{
		SetSpeed(speed);
		//_camera.SetUpMovement(1f);
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
	
	public void PushBack()
	{
		transform.position += Vector3.forward * _pushBackDistance;
	}
	
	public void ResetSpeed()
	{
		_moveSpeed = _baseSpeed;
	}
    
	public void Reset()
	{
		transform.position = _basePosition;
		_lastSavePoint = transform.position;
		ResetSpeed();
	}
}