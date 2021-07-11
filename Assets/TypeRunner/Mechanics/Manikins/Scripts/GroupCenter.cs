using UnityEngine;
using Cinemachine;

namespace TypeRunner
{
	public class GroupCenter : MonoBehaviour
	{
		[SerializeField] private CinemachineTargetGroup group;
		
		private void Update()
		{
			Vector3 newPos = transform.position;
			newPos.z = group.transform.position.z;
			transform.position = newPos;
		}
	}
}