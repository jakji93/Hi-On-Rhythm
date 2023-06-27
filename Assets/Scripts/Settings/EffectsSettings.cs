using Kamgam.SettingsGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsSettings : MonoBehaviour
{
    [SerializeField] private SettingsProvider SettingsProvider;
    [SerializeField] private string ID;

    private float customEffectsVolume;

    // Start is called before the first frame update
    void Start()
    {
        var connection = new GetSetConnection<float>(getter: getCustomEffectsVol, setter: setCustomEffectsVol);
        var setting = SettingsProvider.Settings.GetFloat(ID);
        customEffectsVolume = setting.GetValue();
        customEffectsVolume = customEffectsVolume / 100;
        this.GetComponent<AudioSource>().volume = customEffectsVolume;

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
