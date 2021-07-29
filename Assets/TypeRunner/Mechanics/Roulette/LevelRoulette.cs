using SoundSteppe.RefSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class LevelRoulette : MonoBehaviour, INeedReference
	{
		[SerializeField] private Transform _arrowPos;
		[SerializeField] private Button _claim;
		[SerializeField] private Animator _arrow;
		[SerializeField] private GameObject _holder;
		[SerializeField] private RouletteChunk[] _chunks;
		[SerializeField] private int _activatePerLevel = 5;
		[HideInInspector] public bool IsRouletteActive;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		
		private float _reward = 1f;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
			}
		}
		
		public bool Enable()
		{
			if((_stats.CurrentLevel - 1) % _activatePerLevel == 0)
			{
				_holder.SetActive(true);
				IsRouletteActive = _holder.activeSelf;
			}
			else
			{
				_holder.SetActive(false);
				IsRouletteActive = _holder.activeSelf;
				return false;
			}
			_arrow.enabled = true;
			_claim.interactable = true;
			return true;
		}
		
		public void Roll(System.Action<float> callback)
		{
			_arrow.enabled = false;
			_arrow.SetTrigger("Stop");
			
			RouletteChunk closest = null;
			float smallestDistance = 9999f;
			foreach(var ch in _chunks)
			{
				float currentDistance = Vector3.Distance(ch.transform.position, _arrowPos.position);
				if(closest == null || smallestDistance > currentDistance)
				{
					smallestDistance = currentDistance;
					closest = ch;
				}
			}
			_reward = closest.Reward;
			callback.Invoke(_reward);
		}
	}
}