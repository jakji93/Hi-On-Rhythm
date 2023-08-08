using System.Collections;
using System.Collections.Generic;
using Kamgam.SettingsGenerator;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   [System.Serializable]
   public class WaveContent
   {
      public Transform[] spawnPoints;
      public GameObject[] enemies;
   }

   [System.Serializable]
   public class SpecificWaveContent
   {
      public int waveNumber;
      public WaveContent wave;
   }

   [System.Serializable]
   public class AdvancedWaveContent
   {
      public WaveContent[] wavesContents;
      public SpecificWaveContent[] specificWaveContents;
      public float waveDelay = 1f;
      public int totalWave = 0;
      public float musicTime;
   }

   [SerializeField] private AdvancedWaveContent[] advancedWaveContents;

   [SerializeField] private bool canSpawn = false;

   private float waveTimer;
   private int waveCounter = 0;
   private AdvancedWaveContent curSet;

   private int totalSpawned = 0;

   public static EnemySpawner Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }

   private void Update()
   {
      if(canSpawn) {
         UpdateCurSet();
         waveTimer += Time.deltaTime;
         if(waveTimer > curSet.waveDelay ) {
            waveTimer = 0f;
            SpawnWave();
         }
      }
   }

   private void SpawnWave()
   {
      if (curSet.totalWave == 0) {
         StopSpawn();
         Debug.Log("Total Enemy Spawned: " + totalSpawned);
         return;
      }
      if (waveCounter >= curSet.totalWave) {
         return;
      }
      var curWaveNum = waveCounter % curSet.wavesContents.Length;
      var curWave = curSet.wavesContents[curWaveNum];
      
      foreach( var wave in curSet.specificWaveContents) {
         if(waveCounter+1 == wave.waveNumber ) {
            curWave = wave.wave;
         }
      }

      for( int i = 0; i < curWave.spawnPoints.Length; i++ ) {
         int j = i % curWave.enemies.Length;
         Instantiate(curWave.enemies[j], curWave.spawnPoints[i].position, Quaternion.identity);
         totalSpawned++;
      }
      waveCounter++;
   }

   private void UpdateCurSet()
   {
      var playtime = MusicManager.Instance.GetGameMusicPlaytime();
      if(playtime > curSet.musicTime) {
         foreach(var set in advancedWaveContents ) {
            if(playtime < set.musicTime) {
               curSet = set;
               waveCounter = 0;
               waveTimer = curSet.waveDelay;
               return;
            }
         }
         curSet = advancedWaveContents[advancedWaveContents.Length - 1];
      }
   }

   public void StartSpawn()
   {
      if(advancedWaveContents.IsNullOrEmpty()) {
         canSpawn = false;
         return;
      }
      canSpawn = true;
      curSet = advancedWaveContents[0];
      waveCounter = 0;
      waveTimer = curSet.waveDelay;
   }

   public void StopSpawn()
   {
      canSpawn = false;
   }
}
