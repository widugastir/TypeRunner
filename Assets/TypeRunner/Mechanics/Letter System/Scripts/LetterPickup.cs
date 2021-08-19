using UnityEngine;

namespace TypeRunner
{
	public class LetterPickup : MonoBehaviour
	{
		//------FIELDS
		public E_LetterType letterType;
		[HideInInspector] public E_LetterType LetterType { get; private set; }
		[SerializeField] private Sprite[] _sprites;
		[SerializeField] private float _resetTimer = 0.5f;
		private bool _isPickuped = false;
		
		//------METHODS
		public void Init()
		{
			int length = System.Enum.GetValues(typeof(E_LetterType)).Length;
			int rand = Random.Range(0, length);
			LetterType = (E_LetterType)rand;
			letterType = LetterType;
		}
		
		public void Init(E_LetterType type)
		{
			LetterType = type;
			letterType = LetterType;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(_isPickuped == false && other.TryGetComponent(out Mankin man))
			{
				_isPickuped = true;
				LetterData.Instance.AddLetter(LetterType);
				CancelInvoke(nameof(ResetPickuped));
				Invoke(nameof(ResetPickuped), _resetTimer);
				//Destroy(gameObject);
			}
		}
		
		private void ResetPickuped()
		{
			_isPickuped = false;
		}
	}
}