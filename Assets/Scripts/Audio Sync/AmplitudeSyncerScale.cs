using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AmplitudeSyncerScale : AmplitudeSyncer
{
   [SerializeField] private Vector3 restScale;
   [SerializeField] private Vector3 beatScale;

   public override void OnUpdate()
   {
      base.OnUpdate();
      transform.localScale = beatScale * MusicManager.Instance.GetAmplitude();
   }

   public override void OnBeat()
   {
      base.OnBeat();
   }
}
