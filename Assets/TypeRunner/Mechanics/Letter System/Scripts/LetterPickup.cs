using UnityEngine;

namespace TypeRunner
{
	public class LetterPickup : MonoBehaviour
	{
		//------FIELDS
		[HideInInspector] public E_LetterType LetterType { get; private set; }
		[SerializeField] private Sprite[] _sprites;
		private bool _isPickuped = false;
		
		//------METHODS
		public void Init()
		{
			int length = System.Enum.GetValues(typeof(E_LetterType)).Length;
			int rand = Random.Range(0, length);
			LetterType = (E_LetterType)rand;
		}
		
		public void Init(E_LetterType type)
		{
			LetterType = type;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(_isPickuped == false && other.TryGetComponent(out Mankin man))
			{
				_isPickuped = true;
				LetterData.Instance.AddLetter(LetterType);
				Destroy(gameObject);
			}
		}
	}
}