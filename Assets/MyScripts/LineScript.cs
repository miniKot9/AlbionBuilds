using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LineScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _numText;
	[SerializeField] private SlotScript[] _slots;

	private int _currentId;

	private void Awake()
	{
		for (int i = 0; i < _slots.Length; i++)
		{
			_slots[i].SetId(i);
		}
	}

	public void UpdateLine(int id)
	{
		_currentId = id;
		_numText.text = "" + (_currentId + 1);
		gameObject.name = "" + _currentId;
	}

	public void RemoveThisLine()
	{
		AllActions.RemoveLine(_currentId);
	}

	public void MoveUp()
	{
		AllActions.ListItemsSwap(_currentId, _currentId - 1);
	}
	public void MoveDown()
	{
		AllActions.ListItemsSwap(_currentId, _currentId + 1);
	}

	public LineData GetLineData()
	{
		LineData lineData = new LineData();
		for (int i = 0; i < _slots.Length; i++)
		{
			lineData.Sprites[i] = _slots[i].GetSprite();
		}

		return lineData;
	}

	public void SetLineData(LineData data)
	{
		for (int i = 0; i < _slots.Length; i++)
		{
			if (data.Sprites[i] != null)
				_slots[i].ImageSprite.sprite = data.Sprites[i];
		}
	}
}
