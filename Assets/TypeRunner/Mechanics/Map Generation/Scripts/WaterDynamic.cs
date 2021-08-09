using UnityEngine;

public class WaterDynamic : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Vector2 _offset;

	private void Update()
    {
	    _renderer.material.SetTextureOffset("_MainTex", _offset * Time.time);
    }
}
