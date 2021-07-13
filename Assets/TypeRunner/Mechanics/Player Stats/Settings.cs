using UnityEngine;

namespace TypeRunner
{
	public class Settings : Singleton<Settings>
	{
		//------FIELDS
		[Saveable] public bool Vibration = false;
		[Saveable] public bool Music = false;
	}
}