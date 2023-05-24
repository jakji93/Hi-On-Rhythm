using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameUI : MonoBehaviour
{
   [SerializeField] private Button defaultSelectButton;

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
      gameObject.SetActive(false);
   }

   private void Active()
   {
      gameObject.SetActive(true);
      defaultSelectButton.Select();
   }
}
