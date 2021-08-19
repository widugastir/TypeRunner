using UnityEngine;

public class DoubleGate : MonoBehaviour
{
	[SerializeField] private float _resetTimer = 1.5f;
	[SerializeField] private GameObject _gate;
	
	public void Disable()
	{
		_gate.SetActive(false);
		Invoke(nameof(Enable), _resetTimer);
	}
	
	private void Enable()
	{
		_gate.SetActive(true);
	}
}
