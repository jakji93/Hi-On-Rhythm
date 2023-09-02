using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
   public static SettingsManager Instance { get; private set; }

   [SerializeField] private AudioClip clickClip;
   [SerializeField] private AudioClip buttonClip;
   [SerializeField] private string songSelectionName;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private AudioMixer mixer;

   [SerializeField] private TextMeshProUGUI offsetText;

   private bool isPanelOpen = false;
   private PageController pageController;
   private float offset = 0f;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      gameInput.OnBackPressed += GameInput_OnBackPressed;
      if(PlayerPrefs.HasKey("ChartOffset")) {
         offset = PlayerPrefs.GetFloat("ChartOffset");
      } else {
         offset = 0f;
         PlayerPrefs.SetFloat("ChartOffset", 0f);
      }
      offsetText.text = offset.ToString("F2");
   }

   private void GameInput_OnBackPressed(object sender, System.EventArgs e)
   {
      OnBackButton();
   }

   public void OnBackButton()
   {
      if (!isPanelOpen) {
         GoToSongSelect();
      }
      else {
         pageController.ClosePage();
      }
   }

   public void GoToSongSelect()
   {
      ClipPlayer.Instance.PlayClip(clickClip);
      SceneManager.LoadScene(songSelectionName);
   }

   public void OnButtonClick()
   {
      ClipPlayer.Instance.PlayClip(buttonClip);
   }

   public void OnBottonEnter()
   {
      ClipPlayer.Instance.PlayClip(buttonClip);
   }

   public void SetPanelStatus(bool panelStatus, PageController controller)
   {
      isPanelOpen = panelStatus;
      pageController = controller;
   }

   public void OnSliderChange(float value)
   {
      var normalizedValue = value / 100;
      mixer.SetFloat("SFXVolume", Mathf.Log10(normalizedValue) * 20);
   }

   public void IncreaseOffset()
   {
      offset += 0.01f;
      offset = Mathf.Round(offset * 100f) / 100f;
      offsetText.text = offset.ToString("F2");
      PlayerPrefs.SetFloat("ChartOffset", offset);
   }

   public void DecreaseOffset()
   {
      offset -= 0.01f;
      offset = Mathf.Round(offset * 100f) / 100f;
      offsetText.text = offset.ToString("F2");
      PlayerPrefs.SetFloat("ChartOffset", offset);
   }
}
