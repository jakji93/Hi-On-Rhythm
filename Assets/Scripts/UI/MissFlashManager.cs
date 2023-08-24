using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFlashManager : FlashManager
{
   public static MissFlashManager Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }
}
