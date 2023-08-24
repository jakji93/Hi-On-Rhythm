using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Failed : MonoBehaviour
{
   [SerializeField] private CanvasGroup canvasGroup;
   [SerializeField] private RectTransform buttons;
   [SerializeField] private RectTransform songName;
   [SerializeField] private TextMeshProUGUI failedText;

   public void StartAnimation()
   {
      canvasGroup.DOFade(0.5f, 1f).From().SetEase(Ease.Linear);
      buttons.DOLocalMoveY(0, 1f).From().SetEase(Ease.Linear);
      songName.DOLocalMoveY(0, 1f).From().SetEase(Ease.Linear);
      DOVirtual.Float(-50f, 50f, 1, x =>
      {
         failedText.characterSpacing = x;
      });
   }
}
