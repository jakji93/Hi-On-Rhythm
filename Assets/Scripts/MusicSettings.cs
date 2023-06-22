using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Kamgam.SettingsGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSettings : MonoBehaviour
{
    protected int customMusicVol; // Instead of healthRegeneration this could be super-sampling scale factor X.

    private void Start()
    {
        customMusicVol = (int)this.GetComponent<AudioSource>().volume;
    }

    protected void changeCustomMusicVol(Settings settings)
    {
        var connection = new GetSetConnection<int>
        (
            getter: getCustomMusicVol, // executed if connection.Get() is called.
            setter: setCustomMusicVol  // executed if connection.Set(value) is called.
        );

        var healthSetting = settings.GetOrCreateInt(id: "CustomMusicVol",connection: connection
        );

    }

    protected int getCustomMusicVol()
    {
        return customMusicVol;
    }
    protected void setCustomMusicVol(int value)
    {
        customMusicVol = value;
        //Debug.Log("Health regeneration has been set to: " + value);
    }
}
