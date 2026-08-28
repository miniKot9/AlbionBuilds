using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainList : MonoBehaviour
{
	[SerializeField] private GameObject _lineObject;

	[SerializeField] private Transform _lineParent;


	[SerializeField] private GameObject[] _itemsLists;
	public GameObject[] ItemsLists => _itemsLists;

	[SerializeField] private GameObject[] _weaponsLists;
	public GameObject[] WeaponsLists => _weaponsLists;


	private List<GameObject> _listLines = new List<GameObject>();

	private GameObject _currentItem;

	[SerializeField] private JSONManager _jSONManager;

	private void ItemTaked(Sprite sprite)
	{
		CloseItems();
		if (_currentItem != null)
		{
			_currentItem.GetComponent<Image>().sprite = sprite;
		}
		SaveData();
	}

	public void CloseItems()
	{
		for (int i = 0; i < _itemsLists.Length; i++)
		{
			_itemsLists[i].SetActive(false);
		}
	}
	private void OpenItems(int idItems, GameObject CurrentItem)
	{
		_currentItem = CurrentItem;
		CloseItems();
		_itemsLists[idItems].SetActive(true);
		if (idItems == 0)
		{
			OnOpenWeapons();
		}
	}
	private void OnOpenWeapons()
	{
		for (int i = 0; i < _weaponsLists.Length; i++)
		{
			_weaponsLists[i].SetActive(false);
		}
	}


	//--------------------Lines-----------------------
	private void CreateNewLine()
	{
		GameObject go = Instantiate(_lineObject, _lineParent);
		go.GetComponent<LineScript>().UpdateLine(_listLines.Count);
		_listLines.Add(go);

		UpdateContentHeight();
	}

	private void RemoveLine(int idLine)
	{
		GameObject go = _listLines[idLine];
		_listLines.Remove(_listLines[idLine]);
		Destroy(go);

		UpdateLines();
		SyncHierarchyWithList();
		UpdateContentHeight();
	}

	private void SwapListItems(int index1, int index2)
	{
		bool b = IsSwapedListItems(index1, index2);

		if (b)
		{
			UpdateLines();
			SyncHierarchyWithList();
			UpdateContentHeight();
		} 
	}
	private bool IsSwapedListItems(int index1, int index2)
	{
		if (index1 < 0 || index1 >= _listLines.Count ||
			index2 < 0 || index2 >= _listLines.Count ||
			index1 == index2)
		{
			return false;
		}
		GameObject temp = _listLines[index1];
		_listLines[index1] = _listLines[index2];
		_listLines[index2] = temp;

		return true;
	}

	private void UpdateLines()
	{
		for (int i = 0; i < _listLines.Count; i++)
		{
			_listLines[i].GetComponent<LineScript>().UpdateLine(i);
		}
		SaveData();
		UpdateContentHeight();
	}
	private void SyncHierarchyWithList()
	{
		if (_lineParent == null) return;

		for (int i = 0; i < _listLines.Count; i++)
		{
			if (_listLines[i] != null)
			{
				_listLines[i].transform.SetSiblingIndex(i);
			}
		}
	}
	//------------------------------------------------

	//-------------------Save-------------------------

	private void Start()
	{
		_jSONManager.LoadDefaultData();
	}

	private void OnApplicationQuit()
	{
		
	}

	public void SaveData()
	{
		Debug.Log(_listLines.Count);
		_jSONManager.currentData.LinesData = new LineData[_listLines.Count];

		for (int i = 0; i < _jSONManager.currentData.LinesData.Length; i++)
		{
			_jSONManager.currentData.LinesData[i] = _listLines[i].GetComponent<LineScript>().GetLineData();
		}

		_jSONManager.QuickSave();
	}

	public void LoadData(SavedData data)
	{
		for (int i = 0; i < _listLines.Count; i++)
		{
			Destroy(_listLines[i]);
		}
		_listLines.Clear();

		if (data.LinesData != null)
		{
			for (int i = 0; i < data.LinesData.Length; i++)
			{
				CreateNewLine();
				_listLines[i].GetComponent<LineScript>().SetLineData(data.LinesData[i]);
			}
		}
		UpdateContentHeight();
	}

	//------------------------------------------------

	public float itemHeight = 100f;

	public void UpdateContentHeight()
	{
		//if (_lineParent.GetComponent<ScrollRect>() == null || _lineParent.GetComponent<ScrollRect>().content == null) return;

		RectTransform content = _lineParent.GetComponent<RectTransform>();

		float totalHeight = _lineParent.GetComponent<VerticalLayoutGroup>().padding.top * 2 + (itemHeight * (_listLines.Count + (_listLines.Count > 9? 1 : 0))) + (Mathf.Max(0, _listLines.Count - 1));
		content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
	}


	private void OnDisable()
	{
		AllActions.CreateNewLine = null;
		AllActions.RemoveLine = null;

		AllActions.OpenItems = null;
		AllActions.ItemTaked = null;
		AllActions.CloseItems = null;

		AllActions.ListItemsSwap = null;

	}
	private void OnEnable()
	{
		AllActions.CreateNewLine = CreateNewLine;
		AllActions.RemoveLine = RemoveLine;

		AllActions.OpenItems = OpenItems;
		AllActions.ItemTaked = ItemTaked;
		AllActions.CloseItems = CloseItems;

		AllActions.ListItemsSwap = SwapListItems;

	}
}
