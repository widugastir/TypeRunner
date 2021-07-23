using UnityEngine;

public class ObstacleChild : MonoBehaviour
{
	[SerializeField] private float _lifetime = 2f;
	private Rigidbody _rigi;
	
	protected void Start()
	{
		_rigi = GetComponent<Rigidbody>();
	}
	
	public void Activate(Vector3 forcePoint)
	{
		transform.SetParent(null);
		_rigi.isKinematic = false;
		_rigi.AddForce((transform.position - forcePoint).normalized * 100, ForceMode.Impulse);
		Destroy(gameObject, _lifetime);
    }
}
