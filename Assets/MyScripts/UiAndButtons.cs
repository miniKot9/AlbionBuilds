using UnityEngine;
using UnityEngine.UI;

public class UiAndButtons : MonoBehaviour
{
	[SerializeField] private GameObject _slotObject;

	[SerializeField] private ItemsList[] _itemsLists;

	[SerializeField] private ItemsList[] _weaponsLists;


	private void Start()
	{
		InitImages();
	}

	private void InitImages()
	{
		for (int i = 0; i < GetComponent<MainList>().WeaponsLists.Length; i++)
		{
			for (int x = 0; x < _weaponsLists[i].Sprites.Length; x++)
			{
				GameObject go = Instantiate(_slotObject, GetComponent<MainList>().WeaponsLists[i].transform);
				go.GetComponent<Image>().sprite = _weaponsLists[i].Sprites[x];
			}
		}

		for (int i = 1; i < GetComponent<MainList>().ItemsLists.Length; i++)
		{
			for (int x = 0; x < _itemsLists[i].Sprites.Length; x++)
			{
				GameObject go = Instantiate(_slotObject, GetComponent<MainList>().ItemsLists[i].transform);
				go.GetComponent<Image>().sprite = _itemsLists[i].Sprites[x];
			}
		}
	}

	public void ChooseWeapons(int index)
	{
		for (int i = 0; i < GetComponent<MainList>().WeaponsLists.Length; i++)
		{
			GetComponent<MainList>().WeaponsLists[i].SetActive(false);
		}
		GetComponent<MainList>().WeaponsLists[index].SetActive(true);
	}

	public void CreateNewLine()
	{
		AllActions.CreateNewLine();
	}
}
