using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
   [SerializeField] private AudioClip clickClip;
   [SerializeField] private AudioClip buttonClip;
   [SerializeField] private string songSelectionName;

   public void GoToSongSelect()
   {
      ClipPlayer.Instance.PlayClip(clickClip);
      SceneManager.LoadScene(songSelectionName);
   }

   public void OnButtonClick()
   {
      ClipPlayer.Instance.PlayClip(buttonClip);
   }
}
