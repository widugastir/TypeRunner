using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class Cheats : MonoBehaviour
	{
		[SerializeField] private LevelManager _levelManager;
	    
	    void Update()
		{
			if(Input.GetKeyDown(KeyCode.K))
		    	_levelManager.PreFinishLevel(false, 1);
			if(Input.GetKeyDown(KeyCode.L))
				_levelManager.PreFinishLevel(true, 2, 1f);
	    }
	}
}