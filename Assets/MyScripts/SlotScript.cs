using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{

	private int _idItemsList;
	[SerializeField] private Image _image;
	public Image ImageSprite => _image;


	public void SetId(int id)
	{
		_idItemsList = id;
	}

	public void Choose()
	{
		AllActions.OpenItems(_idItemsList, this.gameObject);
	}

	public Sprite GetSprite()
	{
		return _image.sprite;
	} 

}
