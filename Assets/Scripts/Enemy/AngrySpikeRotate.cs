using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AngrySpikeRotate : MonoBehaviour
{
   [SerializeField] private Transform spikes;

   private int attackCount = 0;

   private void Start()
   {
      NoteManager.Instance.OnAttackBeat += NoteManager_OnAttackBeat;
   }

   private void NoteManager_OnAttackBeat(object sender, EventArgs e)
   {
      attackCount++;
      var targetVec3 = Quaternion.Euler(0f, 0f, attackCount * 45f).eulerAngles;
      spikes.DOKill();
      spikes.DOLocalRotate(targetVec3, 0.1f).SetEase(Ease.Linear);
   }

   private void OnDestroy()
   {
      spikes.DOKill();
      NoteManager.Instance.OnAttackBeat -= NoteManager_OnAttackBeat;
   }
}
