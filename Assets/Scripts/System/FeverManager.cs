using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeverManager : MonoBehaviour, IHasProgress
{
   public class OnFeverModeEventArgs : EventArgs
   {
      public bool isFeverMode;
   }

   public static FeverManager Instance { get; private set; }

   [SerializeField] private float feverPerPerfectHit;
   [SerializeField] private float feverPerGreatHit;
   [SerializeField] private float feverPerGoodHit;
   [SerializeField] private float feverPerNoteMiss;
   [SerializeField] private float feverPerNoHits;
   [SerializeField] private float feverPerEnemyKilled;
   [SerializeField] private MMF_Player feverPlayer;

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
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnNoNoteHits += NoteManager_OnNoNoteHits;
      NoteManager.Instance.OnNotePerfect += NoteManager_OnNotePerfect;
      NoteManager.Instance.OnNoteGreat += NoteManager_OnNoteGreat;
      NoteManager.Instance.OnNoteGood += Instance_OnNoteGood;
      NoteManager.Instance.OnNormal1Hit += NoteManager_OnNormal1Hit;
   }

   private void NoteManager_OnNormal1Hit(object sender, EventArgs e)
   {
      if(isFeverMode) {
         feverPlayer.PlayFeedbacks();
      }
   }

   private void Instance_OnNoteGood(object sender, EventArgs e)
   {
      if (!isFeverMode) {
         UpdateFever(feverPerGoodHit);
      }
   }

   private void NoteManager_OnNoteGreat(object sender, EventArgs e)
   {
      if (!isFeverMode) {
         UpdateFever(feverPerGreatHit);
      }
   }

   private void NoteManager_OnNotePerfect(object sender, EventArgs e)
   {
      if (!isFeverMode) {
         UpdateFever(feverPerPerfectHit);
      }
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
