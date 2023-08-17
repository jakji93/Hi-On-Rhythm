using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
   [SerializeField] private Transform left;
   [SerializeField] private Transform right;
   [SerializeField] private AudioClip sfx;

   private void Start()
   {
      left.DOLocalMoveX(0, 1f).SetEase(Ease.InBack);
      right.DOLocalMoveX(0, 1f).SetEase(Ease.InBack).SetDelay(0.3f).OnComplete(() =>
      {
         ClipPlayer.Instance.PlayClip(sfx);
         transform.DOScale(6, 1f).SetEase(Ease.InOutBack);
      });
   }

   private void OnDestroy()
   {
      DOTween.Clear();
   }
}
