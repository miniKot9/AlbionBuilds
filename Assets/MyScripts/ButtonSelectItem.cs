using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectItem : MonoBehaviour
{

    public void SelectItem()
	{
		AllActions.ItemTaked(gameObject.GetComponent<Image>().sprite);
	}
}
