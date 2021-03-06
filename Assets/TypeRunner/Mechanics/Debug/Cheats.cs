using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TypeRunner
{
	public class Cheats : MonoBehaviour
	{
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private PlayerController _player;
		[SerializeField] private PlayerStats _stats;
	    
	    void Update()
		{
			if(Input.GetKeyDown(KeyCode.K))
		    	_levelManager.PreFinishLevel(false, 1);
			if(Input.GetKeyDown(KeyCode.L))
				_levelManager.PreFinishLevel(true, 2, 1f);
			if(Input.GetKeyDown(KeyCode.F))
				_player.SetImmortal(3f, true);
			if(Input.GetKeyDown(KeyCode.T))
				print(Time.deltaTime);
			if(Input.GetKeyDown(KeyCode.X))
				_stats._successfulWord += 3;
	    }
	}
}