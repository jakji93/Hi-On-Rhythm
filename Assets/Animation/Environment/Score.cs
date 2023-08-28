using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Score : MonoBehaviour
{
   [SerializeField] private CanvasGroup canvasGroup;
   [SerializeField] private RectTransform buttons;
   [SerializeField] private RectTransform songName;
   [SerializeField] private RectTransform difficulty;
   [SerializeField] private RectTransform letterGrade;
   [SerializeField] private RectTransform stats;

   public void StartAnimation()
   {
      canvasGroup.DOFade(0.5f, 1f).From().SetEase(Ease.Linear);
      buttons.DOLocalMoveY(0, 1f).From().SetEase(Ease.Linear);
      songName.DOLocalMoveY(0, 1f).From().SetEase(Ease.Linear);
      difficulty.DOLocalMoveY(0, 1f).From().SetEase(Ease.Linear);
      letterGrade.DOLocalMoveX(0, 1f).From().SetEase(Ease.Linear);
      stats.DOLocalMoveX(0, 1f).From().SetEase(Ease.Linear);
   }
}
