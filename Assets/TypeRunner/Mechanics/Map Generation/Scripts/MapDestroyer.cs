using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class MapDestroyer : MonoBehaviour
	{
		protected void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent<Platform>(out Platform p))
			{
				other.gameObject.SetActive(false);
			}
		}
	}
}