using UnityEngine.EventSystems;
using UnityEngine;

namespace TypeRunner
{
	public class ControlPanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		//------FIELDS
		private float _deltaMove;
		private float _beginPos;
		
		public event System.Action OnStartDrag;
		public event System.Action<float> OnProcessDrag;
		public event System.Action OnStopDrag;
		
		//------METHODS
		public void OnBeginDrag(PointerEventData eventData)
		{
			_beginPos = eventData.position.x / Screen.width;
			OnStartDrag?.Invoke();
		}
		
		public void OnDrag(PointerEventData eventData)
		{
			_deltaMove = eventData.position.x / Screen.width - 0.5f;
			OnProcessDrag?.Invoke(_deltaMove);
		}
		
		public void OnEndDrag(PointerEventData eventData)
		{
			OnStopDrag?.Invoke();
		}
	}
}