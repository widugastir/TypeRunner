using SoundSteppe.RefSystem;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

namespace TypeRunner
{
	public class UpdateTimer : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private TMP_Text _updateText;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private DailyChallenge _daily;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_daily = FindObjectOfType<DailyChallenge>(true);
			}
		}
		
		private void OnEnable()
		{
			StartCoroutine(UpdateUI());
		}
		
		private IEnumerator UpdateUI()
		{
			while(true)
			{
				DateTime now = DateTime.Now;
				DateTime unlockDate = new DateTime(now.Year, now.Month, now.Day, _daily._dailyUpdateHour, 0, 0);
				int currentDay = now.Day;
				if(now > unlockDate)
				{
					unlockDate = unlockDate.AddDays(1);
				}
				TimeSpan span = unlockDate - now;
				_updateText.text = $"Update: {span.Hours}:{span.Minutes}:{span.Seconds}";
				yield return new WaitForSecondsRealtime(1f);
			}
		}
	}
}