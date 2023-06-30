using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
   [SerializeField] private AudioClip clickClip;
   public void GoToSongSelect()
   {
      ClipPlayer.Instance.PlayClip(clickClip);
      SceneManager.LoadScene("Song Selection");
   }
}
