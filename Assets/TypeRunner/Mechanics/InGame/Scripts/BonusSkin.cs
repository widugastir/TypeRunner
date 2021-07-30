using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace TypeRunner
{
	public class BonusSkin : MonoBehaviour, INeedReference
	{
		[SerializeField] private float _fillSpeed = 0.5f;
		[SerializeField] private Image _filler;
		[SerializeField] private bool _addBonus = false;
		
		[SerializeField] private TMP_Text _procentage;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private Shop _shop;
		public static event System.Action OnSkinEarned;
		
		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject == true)
			{
				_stats = FindObjectOfType<PlayerStats>(true);
				_shop = FindObjectOfType<Shop>(true);
			}
		}
		
		private void OnEnable()
		{
			if(_addBonus)
			{
				_procentage.text = $"{((int)(_stats.PrevSkinProgress * 100f)).ToString()}%";
				_filler.fillAmount = _stats.PrevSkinProgress;
				_filler.DOFillAmount(_stats.SkinBonusProgress, _fillSpeed)
				.SetEase(Ease.Linear)
				.SetUpdate(true)
				.OnComplete(FillCompleted);
			}
			else
			{
				_procentage.text = $"{((int)(_stats.SkinBonusProgress * 100f)).ToString()}%";
				_filler.fillAmount = _stats.SkinBonusProgress;
			}
		}
		
		private void FillCompleted()
		{
			_procentage.text = $"{((int)(_stats.SkinBonusProgress * 100f)).ToString()}%";
			if(_stats.SkinBonusProgress >= 1f)
			{
				_procentage.text = $"100%";
				_stats.SkinBonusProgress = 0f;
				//_shop.UnlockRandomSkin();
				OnSkinEarned?.Invoke();
				_filler.fillAmount = 1f;
			}
		}
	}
}