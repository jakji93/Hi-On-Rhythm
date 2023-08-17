using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
   [SerializeField] private Transform left;
   [SerializeField] private Transform right;

   private void Start()
   {
      left.DOLocalMoveX(0, 1f).SetEase(Ease.InBack);
      right.DOLocalMoveX(0, 1f).SetEase(Ease.InBack).SetDelay(0.3f).OnComplete(() =>
      {
         transform.DOScale(6, 1f).SetEase(Ease.InOutBack);
      });
   }

   private void OnDestroy()
   {
      DOTween.Clear();
   }
}
