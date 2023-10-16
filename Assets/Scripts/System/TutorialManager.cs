using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
   [System.Serializable]
   public struct TutorialContent
   {
      public CanvasGroup tipUI;
      public float time;
      public float duration;
   }

   [SerializeField] private TutorialContent[] tutorialTips;
   private int tipOrder = 0;
   private bool isPlaying = false;

   private void Update()
   {
      if (isPlaying) return;
      if (tipOrder < tutorialTips.Length) {
         var playtime = MusicManager.Instance.GetGameMusicPlaytime();
         if (playtime > tutorialTips[tipOrder].time) {
            isPlaying = true;
            tutorialTips[tipOrder].tipUI.DOFade(1, 0.2f).From(0).OnComplete(() =>
            {
               tutorialTips[tipOrder].tipUI.DOFade(0, 0.2f).From(1).SetDelay(tutorialTips[tipOrder].duration).OnComplete(() =>
               {
                  tipOrder++;
                  isPlaying = false;
               });
            });
         }
      }
   }
}
