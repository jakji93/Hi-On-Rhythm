using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using System.Threading.Tasks;

public class PauseGameUI : MonoBehaviour
{
   [SerializeField] private Button defaultSelectButton;
   [SerializeField] private GameObject pauseGameScreen;
   [SerializeField] private GameObject pauseTab;
   [SerializeField] private Image pauseBG;

   private float defaultBDAlpha;

   private void Start()
   {
      GameplayManager.Instance.OnGamePause += GameplayManager_OnGamePause;
      GameplayManager.Instance.OnGameUnpause += GameplayManager_OnGameUnpause;
      defaultBDAlpha = pauseBG.color.a;
      Debug.Log(defaultBDAlpha);
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

   async private void Hide()
   {
      var task = new List<Task>();
      task.Add(pauseTab.transform.DOScale(0, 0.1f).SetUpdate(true).AsyncWaitForCompletion());
      task.Add(pauseBG.DOFade(0, 0.2f).SetUpdate(true).AsyncWaitForCompletion());
      await Task.WhenAll(task);
      pauseGameScreen.SetActive(false);
   }

   private void Active()
   {
      pauseGameScreen.SetActive(true);
      defaultSelectButton.Select();
      pauseTab.transform.localScale = Vector3.zero;
      pauseBG.DOFade(defaultBDAlpha, 0.2f).SetUpdate(true);
      pauseTab.transform.DOScale(1, 0.3f).SetUpdate(true);
   }
}
