using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioSyncerScale : AudioSyncer
{
   [SerializeField] private float restScale;
   [SerializeField] private float beatScale;

   public override void OnUpdate()
   {
      base.OnUpdate();

      if (isBeat) return;
   }

   public override void OnBeat()
   {
      base.OnBeat();

      transform.DOKill();
      transform.DOScaleY(beatScale, timeToBeat).OnComplete(() =>
      {
         isBeat = false;
         transform.DOScaleY(restScale, restSmoothTime).SetEase(Ease.OutQuad);
      });
   }
}
