using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PageController : MonoBehaviour
{
   [SerializeField] private RectTransform page;
   [SerializeField] private CanvasGroup canvasGroup;
   [SerializeField] private AudioClip openSFX;
   [SerializeField] private AudioClip closeSFX;

   public void OpenPage()
   {
      page.gameObject.SetActive(true);
      canvasGroup.DOFade(1, 0.1f).SetEase(Ease.Linear);
      ClipPlayer.Instance.PlayClip(openSFX);
      SettingsManager.Instance.SetPanelStatus(true, this);
   }

   public void ClosePage()
   {
      ClipPlayer.Instance.PlayClip(closeSFX);
      canvasGroup.DOFade(0.3f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
      {
         page.gameObject.SetActive(false);
         SettingsManager.Instance.SetPanelStatus(false, null);
      });
   }
}
