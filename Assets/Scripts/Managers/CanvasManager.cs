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
	public Button NextTurnButton;

	public Slider CurrentStaminaSlider;
	
	public Text CurrentStaminaText;
	public Text StaminaStat;

	public Slider StaminaStatSlider;

	public static CanvasManager instance;
	
	private void Awake() {
		instance = this;
		BurnButton.onClick.AddListener(()=>SelectionStateManager.instance.ToggleBurn());
		TackeButton.onClick.AddListener(()=>SelectionStateManager.instance.ToggleTackle());
		NextTurnButton.onClick.AddListener(()=>SelectionStateManager.instance.EndTurn());
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

	public void setStaminaUI(double currentStamina, double staminaStat)
	{
		CurrentStaminaText.text = currentStamina.ToString("N1") + " /";
		StaminaStat.text = staminaStat.ToString("N1");

		StaminaStatSlider.value = (float) staminaStat;
		CurrentStaminaSlider.value = (float) currentStamina;

	}

	public void ResetButtonUIColor()
	{
		TackeButton.image.color = Color.white;
		BurnButton.image.color = Color.white;
	}

}
