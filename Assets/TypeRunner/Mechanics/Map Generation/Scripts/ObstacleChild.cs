using UnityEngine;

public class ObstacleChild : MonoBehaviour
{
	[SerializeField] private float _lifetime = 2f;
	[SerializeField] private float _force = 100f;
	private Rigidbody _rigi;
	
	protected void Start()
	{
		_rigi = GetComponent<Rigidbody>();
	}
	
	public void Activate(Vector3 forcePoint)
	{
		transform.SetParent(null);
		_rigi.isKinematic = false;
		_rigi.AddForce((transform.position - forcePoint).normalized * _force + Vector3.up * _force, ForceMode.Impulse);
		_rigi.AddTorque(Random.insideUnitSphere * 150f);
		Destroy(gameObject, _lifetime);
    }
}