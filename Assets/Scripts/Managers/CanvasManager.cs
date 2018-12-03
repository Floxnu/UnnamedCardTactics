using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public Slider APSlider;
	public Text APText;
	public Image CardSectionBackground;
	public Button BurnButton;
	public Button TackeButton;
	public static CanvasManager instance;
	
	private void Awake() {
		instance = this;
		BurnButton.onClick.AddListener(()=>SelectionStateManager.instance.ToggleBurn());
		TackeButton.onClick.AddListener(()=>SelectionStateManager.instance.ToggleTackle());
		EnableCharacterUI(false);
	}

	public void EnableCharacterUI(bool active){
		APSlider.gameObject.SetActive(active);
		APText.enabled = active;
		CardSectionBackground.enabled = active;
		BurnButton.gameObject.SetActive(active);
		TackeButton.gameObject.SetActive(active);
	}

	public void setAPText(string text)
	{
		APText.text = text;
	}

	public void setCurrentAP(float ap)
	{
		APSlider.value = ap;
	}

	public void ResetButtonUIColor()
	{
		TackeButton.image.color = Color.white;
		BurnButton.image.color = Color.white;
	}

}
