using SoundSteppe.RefSystem;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace TypeRunner
{
	public class ColorRoulette : MonoBehaviour, INeedReference
	{
		//------FIELDS
		[SerializeField] private int _updateCost = 50;
		[SerializeField] private float _rotateSpeed = 1f;
		[SerializeField] private Transform _arrow;
		[SerializeField] private Button _button;
		[SerializeField] private GameObject _costIcon;
		[SerializeField] private GameObject _textColor;
		[SerializeField, HideInInspector] private ColorChanger _colorChanger;
		[SerializeField, HideInInspector] private PlayerStats _stats;
		[SerializeField, HideInInspector] private ColorPoint[] _points;
		[SerializeField, HideInInspector] private CoinManager _coins;
		private bool _canRoll = true;

		//------METHODS
		public void UpdateReferences(bool sceneObject)
		{
			if(sceneObject)
			{
				_points = gameObject.GetComponentsInChildren<ColorPoint>();
				_colorChanger = FindObjectOfType<ColorChanger>(true);
				_stats = FindObjectOfType<PlayerStats>(true);
				_coins = FindObjectOfType<CoinManager>(true);
			}
		}
		
		public void Spin()
		{
			if(_canRoll == false)
				return;
			_canRoll = false;
			if(_stats._freeColorRoulette == true)
			{
				_stats._freeColorRoulette = false;
			}
			else
			{
				if(_coins.TrySpend(_updateCost) == false)
				{
					return;
				}
				_stats._freeColorRoulette = true;
			}
			float currentZ = transform.rotation.eulerAngles.z;
			currentZ -= 323.5f;
			transform.DOLocalRotate(new Vector3(0f, 0f, currentZ), _rotateSpeed, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear)
				.SetUpdate(true)
				.OnComplete(Complete);
			UpdateUI();
		}
		
		private void OnEnable()
		{
			SaveSystem.OnEndLoad += OnLoad;
			UpdateUI();
		}
		
		private void OnDisable()
		{
			SaveSystem.OnEndLoad -= OnLoad;
			transform.DOKill(true);
			_canRoll = true;
		}
		
		private void Complete()
		{
			_stats._rouletteRotate = transform.rotation.eulerAngles.z;
			Color newColor = GetSelectedColor();
			_colorChanger.SetPlayerColor(newColor);
			_canRoll = true;
		}
		
		private Color GetSelectedColor()
		{
			float minDistance = 99999f;
			ColorPoint closestPoint = null;
			foreach(var p in _points)
			{
				float distance = Vector3.Distance(_arrow.position, p.transform.position);
				if(closestPoint == null || distance < minDistance)
				{
					closestPoint = p;
					minDistance = distance;
				}
			}
			return closestPoint.ManikinColor;
		}
		
		private void OnLoad()
		{
			if(_stats._rouletteRotate != -1f)
			{
				Vector3 rotation = transform.rotation.eulerAngles;
				rotation.z = _stats._rouletteRotate;
				transform.rotation = Quaternion.Euler(rotation);
			}
			UpdateUI();
		}
		
		private void UpdateUI()
		{
			if(_stats.Coins >= _updateCost || _stats._freeColorRoulette)
			{
				_button.interactable = true;
			}
			else
			{
				_button.interactable = false;
			}
			
			if(_stats._freeColorRoulette == false)
			{
				_costIcon.SetActive(true);
				_textColor.SetActive(false);
			}
			else
			{
				_costIcon.SetActive(false);
				_textColor.SetActive(true);
			}
		}
	}
}