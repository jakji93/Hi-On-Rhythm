using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToturialNavigator : MonoBehaviour
{
   [SerializeField] private GameObject chart;
   [SerializeField] private GameObject spawner;
   [SerializeField] private AudioClip gameMusic;
   [SerializeField] private string songName;
   [SerializeField] private string displayName;
   [SerializeField] private int bpm;
   [SerializeField] private string sceneName;

   public void GoToTutorial()
   {
      SongLoader.Instance.SetBPM(bpm);
      SongLoader.Instance.SetChart(chart);
      SongLoader.Instance.SetSpawner(spawner);
      SongLoader.Instance.SetGameMusic(gameMusic);
      SongLoader.Instance.SetDisplayName(displayName);
      SongLoader.Instance.SetSongName(songName);
      Loader.Load2(sceneName);
   }
}
