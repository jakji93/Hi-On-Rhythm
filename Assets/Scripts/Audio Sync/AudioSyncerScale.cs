using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioSyncerScale : AudioSyncer
{
   [SerializeField] private Vector3 restScale;
   [SerializeField] private Vector3 beatScale;

   public override void OnUpdate()
   {
      base.OnUpdate();

   }

   public override void OnBeat()
   {
      base.OnBeat();

      transform.DOKill();
      transform.DOScale(beatScale, timeToBeat).OnComplete(() =>
      {
         isBeat = false;
         transform.DOScale(restScale, restSmoothTime).SetEase(Ease.OutQuad);
      });
   }

   private void OnDestroy()
   {
      transform.DOKill();
   }
}
