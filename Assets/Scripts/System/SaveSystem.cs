using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
   public static SaveSystem Instance;

   private void Awake()
   {
      Instance = this;
   }

   public bool TrySaveHighScore(ScoreStruct score, SongNames songName, Difficulties difficulty)
   {
      var fileName = songName.ToString() + '_' + difficulty.ToString();
      if(ES3.KeyExists(fileName)) {
         var currHighScore = ES3.Load<ScoreStruct>(fileName);
         if(score.score > currHighScore.score) {
            ES3.Save(fileName, score);
            return true;
         }
         return false;
      } else {
         ES3.Save(fileName, score);
         return true;
      }
   }

   public bool TryLoadHighScore(SongNames songName, Difficulties difficulty, out ScoreStruct score)
   {
      var fileName = songName.ToString() + '_' + difficulty.ToString();
      score = new();
      if (ES3.KeyExists(fileName)) {
         score = ES3.Load<ScoreStruct>(fileName);
         return true;
      }
      return false;
   }
}
