using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Kamgam.SettingsGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicSettings : MonoBehaviour
{
    private float customMusicVol;

    [SerializeField] private SettingsProvider SettingsProvider;
    [SerializeField] private string ID;

    private void Start()
    {
        var connection = new GetSetConnection<float>(getter: getCustomMusicVol, setter: setCustomMusicVol);
        var setting = SettingsProvider.Settings.GetFloat(ID);
        customMusicVol = setting.GetValue();
        customMusicVol = customMusicVol / 100;
        this.GetComponent<AudioSource>().volume = customMusicVol ;
        
        print(customMusicVol);
    }

    protected void changeCustomMusicVol(Settings settings)
    {
        //var connection = new GetSetConnection<float>(getter: getCustomMusicVol,setter: setCustomMusicVol);

       // var customMusicVolSetting = settings.GetOrCreateFloat(id: "customMusicVol",connection: connection);

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
}
