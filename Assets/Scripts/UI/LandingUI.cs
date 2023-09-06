using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LandingUI : MonoBehaviour
{
   [SerializeField] private CanvasGroup buttons;
   [SerializeField] private CanvasGroup links;

   private bool isOpened = false;
   private Transform buttonTransform;
   private Transform linksTransform;

   private void Start()
   {
      buttonTransform = buttons.transform;
      linksTransform = links.transform;
   }

   public void OnButtonPressed()
   {
      if (isOpened) {
         buttonTransform.DOKill();
         isOpened = false;
         buttonTransform.DOLocalMoveX(118, 0.5f).From(380f).SetEase(Ease.InOutBack);
         linksTransform.DOKill();
         linksTransform.DOLocalMoveY(-190, 0.5f).From(-300f).SetEase(Ease.InOutBack);
         buttons.DOKill();
         buttons.DOFade(0, 0.5f).From(1).SetEase(Ease.Linear);
         links.DOKill();
         links.DOFade(0, 0.5f).From(1).SetEase(Ease.Linear);
      } else {
         buttonTransform.DOKill();
         isOpened = true;
         buttonTransform.DOLocalMoveX(380, 0.5f).From(118f).SetEase(Ease.InOutBack);
         linksTransform.DOKill();
         linksTransform.DOLocalMoveY(-300, 0.5f).From(-190f).SetEase(Ease.InOutBack);
         buttons.DOKill();
         buttons.DOFade(1, 0.5f).From(0).SetEase(Ease.Linear);
         links.DOKill();
         links.DOFade(1, 0.5f).From(0).SetEase(Ease.Linear);
      }
   }
}
