using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HitFlashManager : FlashManager
{
   public static HitFlashManager Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }
}
