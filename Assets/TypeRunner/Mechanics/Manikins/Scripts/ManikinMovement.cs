using UnityEngine;

namespace TypeRunner
{
	public class ManikinMovement : MonoBehaviour
	{
		//------FIELDS
		[SerializeField] private Rigidbody _rigi;
		
		//------METHODS
		public void Move(Vector3 force)
		{
			_rigi.MovePosition(transform.position + force * Time.deltaTime);
		    //_rigi.AddForce(force, ForceMode.Acceleration);
	    }
	}
}