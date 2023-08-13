using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class AudioSyncerShake : AudioSyncer
{
   [SerializeField] private MMF_Player mmfPlayer;

   public override void OnBeat()
   {
      base.OnBeat();
      mmfPlayer.PlayFeedbacks();
   }
}
