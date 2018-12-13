using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	[Header("AP")]
	public Slider APSlider;
	public Text APText;

	[Header("Stamina")]
	public Text CurrentStaminaText;
	public Text StaminaStat;
	public Slider CurrentStaminaSlider;
	public Slider StaminaStatSlider;

	[Header("Buttons")]
	public Button BurnButton;
	public Button TackeButton;
	public Button NextTurnButton;

	[Header("Images")]
	public Image CardSectionBackground;
	public GameObject LibraryDisplay;
	public GameObject DiscardDisplay;

	private Text LibraryNumberText;
	private Text DiscardNumberText;


	public static CanvasManager instance;
	
	private void Awake() {
		instance = this;

		LibraryNumberText = LibraryDisplay.GetComponentInChildren<Text>();
		DiscardNumberText = DiscardDisplay.GetComponentInChildren<Text>();

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

		LibraryDisplay.SetActive(active);
		DiscardDisplay.SetActive(active);
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

	public void UpdateDeckUI(int LibraryCards, int DiscardCards){
		LibraryNumberText.text = LibraryCards.ToString();
		DiscardNumberText.text = DiscardCards.ToString();
	}

}
