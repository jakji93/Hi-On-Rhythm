using System.Collections;
using System.Collections.Generic;
using Kamgam.SettingsGenerator;
using UnityEngine;

namespace Kamgam.SettingsGenerator
{
   public class SpecialEffectConnection : Connection<bool>
   {
      public override bool Get()
      {
         if (PlayerPrefs.HasKey("SpecialEffects")) {
            return PlayerPrefs.GetInt("SpecialEffects") == 1;
         } else {
            PlayerPrefs.SetInt("SpecialEffects", 1);
            return true;
         }
      }

      public override void Set(bool value)
      {
         PlayerPrefs.SetInt("SpecialEffects", value ? 1 : 0);
         NotifyListenersIfChanged(value);
      }
   }
}

