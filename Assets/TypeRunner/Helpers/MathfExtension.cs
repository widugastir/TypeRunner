using UnityEngine;

public static class MathfExtension
{
	public static Vector3 Vector3NotX = new Vector3(){x = 0f, y = 1f, z = 1f};
	public static Vector3 Vector3NotY = new Vector3(){x = 1f, y = 0f, z = 1f};
	public static Vector3 Vector3NotZ = new Vector3(){x = 1f, y = 1f, z = 0f};
	
	public static bool IsInRange(float value, float min, float max)
	{
		if(min <= value && max >= value)
			return true;
		return false;
	}
}
