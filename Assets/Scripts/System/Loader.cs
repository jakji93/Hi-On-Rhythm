using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
   private static string targetScene;

   public static void Load(string sceneName)
   {
      targetScene = sceneName;
      SceneManager.LoadScene("Loading");
   }

   public static void LoadCallback()
   {
      SceneManager.LoadScene(targetScene);
   }
}
