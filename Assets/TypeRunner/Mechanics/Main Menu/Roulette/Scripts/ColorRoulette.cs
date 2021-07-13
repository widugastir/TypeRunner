using SoundSteppe.RefSystem;
using UnityEngine;
using DG.Tweening;

namespace TypeRunner
{
	public class ColorRoulette : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private float _rotateSpeed = 1f;
		[SerializeField] private Transform _arrow;
		//[SerializeField, HideInInspector] private OwnerPool _ownerPool;
		//[SerializeField, HideInInspector] private ColorPoint[] _points;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			//_points = gameObject.GetComponentsInChildren<ColorPoint>();
			//if(sceneObject == true)
			//	_ownerPool = FindObjectOfType<OwnerPool>(true);
		}
		
		public void Spin()
		{
			float currentZ = transform.rotation.eulerAngles.z;
			currentZ -= 323.5f;
			transform.DOLocalRotate(new Vector3(0f, 0f, currentZ), _rotateSpeed, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear)
				.SetUpdate(true)
				.OnComplete(Complete);
		}
		
		private void OnDisable()
		{
			transform.DOKill();
		}
		
		private void Complete()
		{
			//Color newColor = GetSelectedColor();
			//_ownerPool.SetColorTo(E_Owner.player, newColor);
		}
		
		//private Color GetSelectedColor()
		//{
			//float minDistance = 99999f;
			//ColorPoint closestPoint = null;
			//foreach(var p in _points)
			//{
			//	float distance = Vector3.Distance(_arrow.position, p.transform.position);
			//	if(closestPoint == null || distance < minDistance)
			//	{
			//		closestPoint = p;
			//		minDistance = distance;
			//	}
			//}
			//return closestPoint.ManikinColor;
		//}
	}
}