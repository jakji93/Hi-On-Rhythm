using Kamgam.SettingsGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectsSettings : MonoBehaviour
{
   [SerializeField] private SettingsProvider SettingsProvider;
   [SerializeField] private string ID;
   [SerializeField] private AudioMixer mixer;

   private float customEffectsVolume;

   // Start is called before the first frame update
   void Start()
   {
      var connection = new GetSetConnection<float>(getter: getCustomEffectsVol, setter: setCustomEffectsVol);
      var setting = SettingsProvider.Settings.GetFloat(ID);
      customEffectsVolume = setting.GetValue();
      customEffectsVolume = customEffectsVolume / 100;
      mixer.SetFloat("SFXVolume", Mathf.Log10(customEffectsVolume) * 20);

      print(customEffectsVolume);

   }

   private float getCustomEffectsVol()
   {
      return customEffectsVolume;
   }

   void setCustomEffectsVol(float value)
   {
      customEffectsVolume = value;
      this.GetComponent<AudioSource>().volume = customEffectsVolume;
   }
}
