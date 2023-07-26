using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;

public class PauseGameUI : MonoBehaviour
{
   [SerializeField] private Button defaultSelectButton;
   [SerializeField] private GameObject pauseGameScreen;

   private void Start()
   {
      GameplayManager.Instance.OnGamePause += GameplayManager_OnGamePause;
      GameplayManager.Instance.OnGameUnpause += GameplayManager_OnGameUnpause;
      Hide();
   }

   private void GameplayManager_OnGameUnpause(object sender, System.EventArgs e)
   {
      Hide();
   }

   private void GameplayManager_OnGamePause(object sender, System.EventArgs e)
   {
      Active();
   }

   private void Hide()
   {
      pauseGameScreen.SetActive(false);
   }

   private void Active()
   {
      pauseGameScreen.SetActive(true);
      defaultSelectButton.Select();
   }
}
