﻿using UnityEngine.EventSystems;
using SoundSteppe.RefSystem;
using UnityEngine;

namespace TypeRunner
{
	public class ControlPanel : MonoBehaviour, INeedReference, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		//------FIELDS
		private float _deltaMove;
		private float _beginPos;
		[SerializeField, HideInInspector] private PlayerController _player;
		
		public event System.Action OnStartDrag;
		public event System.Action<float> OnProcessDrag;
		public event System.Action OnStopDrag;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_player = FindObjectOfType<PlayerController>();
			}
		}
		
		public void OnBeginDrag(PointerEventData eventData)
		{
			_beginPos = eventData.position.x / Screen.width;
			_player.OnStartDrag();
			OnStartDrag?.Invoke();
		}
		
		public void OnDrag(PointerEventData eventData)
		{
			_deltaMove = eventData.position.x / Screen.width - 0.5f;
			_player.OnProcessDrag(_deltaMove);
			OnProcessDrag?.Invoke(_deltaMove);
		}
		
		public void OnEndDrag(PointerEventData eventData)
		{
			OnStopDrag?.Invoke();
			_player.OnEndDrag();
		}
	}
}