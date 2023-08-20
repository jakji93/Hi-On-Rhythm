using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
   public static SettingsManager Instance { get; private set; }

   [SerializeField] private AudioClip clickClip;
   [SerializeField] private AudioClip buttonClip;
   [SerializeField] private string songSelectionName;
   [SerializeField] private GameInput gameInput;

   private bool isPanelOpen = false;
   private PageController pageController;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      gameInput.OnBackPressed += GameInput_OnBackPressed;
   }

   private void GameInput_OnBackPressed(object sender, System.EventArgs e)
   {
      if (!isPanelOpen) {
         GoToSongSelect();
      } else {
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
}
