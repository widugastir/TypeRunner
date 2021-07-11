using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using NaughtyAttributes;

namespace TypeRunner
{
	public class LevelManager : MonoBehaviour
	{
		[SerializeField, Scene] private string _sceneToLoad;
		[SerializeField] private GameObject _levelEndCanvas;
		
		public void FinishLevel()
		{
			Time.timeScale = 0f;
			_levelEndCanvas.SetActive(true);
		}
		
		public void RestartLevel()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(_sceneToLoad);
		}
	}
}