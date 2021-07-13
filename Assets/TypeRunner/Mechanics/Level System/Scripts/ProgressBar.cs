using UnityEngine.UI;
using UnityEngine;

namespace TypeRunner
{
	public class ProgressBar : MonoBehaviour
	{
		//------FIELDS
		//[SerializeField] private MainTower _playerTower;
		//[SerializeField] private MainTower _enemyTower;
		[SerializeField] private Image _imageFiller;
		private int _playerHealth = 0;
		private int _enemyHealth = 0;
		
		//------METHODS
		private void OnEnable()
		{
			Reset();
			//MainTower.OnHealthChange += OnHealthChange;
			//MainTower.OnInit += OnHealthChange;
		}
		
		private void OnDisable()
		{
			//MainTower.OnHealthChange -= OnHealthChange;
			//MainTower.OnInit -= OnHealthChange;
		}
		
		private void Reset()
		{
			//_playerHealth = _playerTower.Health;
			//_enemyHealth = _enemyTower.Health;
			UpdateUI();
		}
		
		//private void OnHealthChange(MainTower tower)
		//{
		//	if(tower.Owner == E_Owner.player)
		//		_playerHealth = tower.Health;
		//	else if(tower.Owner == E_Owner.bot)
		//		_enemyHealth = tower.Health;
		//	UpdateUI();
		//}
		
		private void UpdateUI()
		{
			float healthSumm = _playerHealth + _enemyHealth;
			_imageFiller.fillAmount = _playerHealth / healthSumm;
		}
	}
}