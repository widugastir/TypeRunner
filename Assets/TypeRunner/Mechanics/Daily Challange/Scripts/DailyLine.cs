using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TypeRunner
{
	public class DailyLine : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private TMP_Text _percentageText;
		[SerializeField] private TMP_Text _rewardText;
		[SerializeField] private TMP_Text _humans;
		[SerializeField] private Image _completeImage;
		[SerializeField] private GameObject _highlight;
		[SerializeField] private Sprite _completed;
		[SerializeField] private Sprite _notCompleted;
		private DailyReward _reward;
		
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private DailyChallenge _challenge;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_challenge = FindObjectOfType<DailyChallenge>(true);
			}
		}
		
		private void OnEnable()
		{
			UpdateUI();
		}
		
		public void Init(DailyReward reward)
		{
			_reward = reward;
		}
		
		public void UpdateUI()
		{
			_percentageText.text = _reward.Percentage.ToString() + "%";
			_rewardText.text = _reward.Coins.ToString();
			_humans.text = _reward.Humans.ToString();
			
			if(_challenge.IsCurrentReward(_reward))
			{
				_highlight.SetActive(true);
			}
			
			else
			{
				_highlight.SetActive(false);
			}
			
			if(_stats.DailyProcentage >= _reward.Percentage)
			{
				_completeImage.sprite = _completed;
			}
			else
			{
				_completeImage.sprite = _notCompleted;
			}
		}
	}
}