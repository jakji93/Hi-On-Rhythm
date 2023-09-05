using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class LandingPageManager : MonoBehaviour
{
   [SerializeField] private string songSelecionSceneName;
   [SerializeField] private TextMeshProUGUI versionText;
   [SerializeField] private AudioSource mainAudioSource;
   [SerializeField] private GameInput gameInput;

   [Header("Quit Panel")]
   [SerializeField] private Transform quitScreen;
   [SerializeField] private Image backGround;
   [SerializeField] private CanvasGroup quitPanel;
   [SerializeField] private float quitPanelVolume;

   private float defaultBDAlpha;
   private float defaultAuidioVol;

   public void QuitGame()
   {
      Application.Quit();
   }

   public void GoToSongSelection()
   {
      Loader.Load(songSelecionSceneName);
   }

   public void GoToTwitter()
   {
      Application.OpenURL("https://twitter.com/FeverNightDev");
   }

   public void GoToYoutube()
   {
      Application.OpenURL("https://www.youtube.com/channel/UCxcwEa1euwIV-jmcLQO40mQ");
   }

   public void GoToSteam()
   {
      //TODO: Open SteamWishlist
      //Application.OpenURL("");
   }

   public void OpenQuit()
   {
      defaultAuidioVol = mainAudioSource.volume;
      if (quitPanelVolume < mainAudioSource.volume) {
         mainAudioSource.DOFade(quitPanelVolume, 0.1f);
      }
      quitScreen.gameObject.SetActive(true);
      quitPanel.alpha = 0;
      backGround.DOFade(defaultBDAlpha, 0.2f).SetUpdate(true);
      quitPanel.DOFade(1f, 0.3f);
   }

   async public void CloseQuit()
   {
      if (defaultAuidioVol > mainAudioSource.volume) {
         mainAudioSource.DOFade(defaultAuidioVol, 0.3f);
      }
      var tasks = new List<Task>();
      tasks.Add(quitPanel.DOFade(0f, 0.1f).AsyncWaitForCompletion());
      tasks.Add(backGround.DOFade(0, 0.2f).AsyncWaitForCompletion());
      await Task.WhenAll(tasks);
      quitScreen.gameObject.SetActive(false);
   }

   private void Start()
   {
      versionText.text = "v" + Application.version;
      defaultBDAlpha = backGround.color.a;
      var color = backGround.color;
      color.a = 0f;
      backGround.color = color;
      gameInput.OnBackPressed += GameInput_OnBackPressed;
      gameInput.OnStartPressed += GameInput_OnStartPressed;
   }

   private void GameInput_OnStartPressed(object sender, System.EventArgs e)
   {
      if(quitScreen.gameObject.activeSelf) {
         QuitGame();
      } else {
         GoToSongSelection();
      }
   }

   private void GameInput_OnBackPressed(object sender, System.EventArgs e)
   {
      if (quitScreen.gameObject.activeSelf) {
         CloseQuit();
      }
      else {
         OpenQuit();
      }
   }
}
