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

   public static void Load2(string sceneName)
   {
      targetScene = sceneName;
      SceneManager.LoadScene("Loading 2");
   }

   public static void LoadCallback()
   {
      SceneManager.LoadScene(targetScene);
   }
}
