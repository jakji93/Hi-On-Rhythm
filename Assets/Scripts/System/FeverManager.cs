using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeverManager : MonoBehaviour, IHasProgress
{
   public class OnFeverModeEventArgs : EventArgs
   {
      public bool isFeverMode;
   }

   public static FeverManager Instance { get; private set; }

   [SerializeField] private float feverPerNoteHit;
   [SerializeField] private float feverPerNoteMiss;
   [SerializeField] private float feverPerNoHits;
   [SerializeField] private float feverPerEnemyKilled;

   private float curFever = 0;
   private float maxFever = 100;
   private bool isFeverMode = false;

   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
   public event EventHandler<OnFeverModeEventArgs> OnFeverModeChanged;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NoteManager.Instance.OnNormal1Hit += NoteManager_OnNormal1Hit;
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnNoNoteHits += NoteManager_OnNoNoteHits;
   }

   private void NoteManager_OnNoNoteHits(object sender, EventArgs e)
   {
      if (!isFeverMode) {
         UpdateFever(feverPerNoHits);
      }
   }

   private void NoteManager_OnNoteMissed(object sender, EventArgs e)
   {
      if (!isFeverMode) {
         UpdateFever(feverPerNoteMiss);
      }
   }

   private void NoteManager_OnNormal1Hit(object sender, EventArgs e)
   {
      if(!isFeverMode) {
         UpdateFever(feverPerNoteHit);
      }
   }

   public void EnemyKilled()
   {
      if (!isFeverMode) {
         var value = (1f + ComboManager.Instance.GetMultiplier() / 10f) * feverPerEnemyKilled;
         UpdateFever(value);
      }
   }

   private void UpdateFever(float amount)
   {
      curFever = curFever + amount;
      curFever = Mathf.Clamp(curFever, 0, maxFever);
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
      {
         currentValue = curFever,
         maxValue = maxFever
      });
      if(curFever >= maxFever) {
         isFeverMode = true;
         OnFeverModeChanged?.Invoke(this, new OnFeverModeEventArgs
         {
            isFeverMode = true
         });
         DOVirtual.Float(maxFever, 0f, 6f, x =>
         {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
               currentValue = x,
               maxValue = maxFever
            });
         }).SetEase(Ease.Linear).OnComplete(() =>
         {
            curFever = 0;
            isFeverMode = false;
            OnFeverModeChanged?.Invoke(this, new OnFeverModeEventArgs
            {
               isFeverMode = false
            });
         });
      }
   }
}
