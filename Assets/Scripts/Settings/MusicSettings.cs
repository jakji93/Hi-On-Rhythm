using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Kamgam.SettingsGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSettings : MonoBehaviour
{
   private float customMusicVol;

   [SerializeField] private SettingsProvider SettingsProvider;
   [SerializeField] private string ID;
   [SerializeField] private AudioMixer mixer;

   private void Start()
   {
      var connection = new GetSetConnection<float>(getter: getCustomMusicVol, setter: setCustomMusicVol);
      var setting = SettingsProvider.Settings.GetFloat(ID);
      customMusicVol = setting.GetValue();
      customMusicVol = customMusicVol / 100;
      mixer.SetFloat("MusicVolume", Mathf.Log10(customMusicVol)*20);
   }

   protected float getCustomMusicVol()
   {
      return customMusicVol;
   }
   protected void setCustomMusicVol(float value)
   {
      customMusicVol = value;
      this.GetComponent<AudioSource>().volume = customMusicVol;
   }

   public void OnSliderChange(float value)
   {
      var normalizedValue = value / 100;
      mixer.SetFloat("MusicVolume", Mathf.Log10(normalizedValue) * 20);
   }
}
