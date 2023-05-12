using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   [System.Serializable]
   public class WaveContent
   {
      public Transform[] spawnPoints;
      public GameObject[] enemies;
   }

   [SerializeField] private WaveContent[] waveContent;
   [SerializeField] private float waveDelay = 1f;

   [SerializeField] private bool canSpawn = false;

   private float waveTimer;
   private int waveCounter = 0;

   public static EnemySpawner Instance { get; private set; }

   private void Start()
   {
      Instance = this;
      waveTimer = waveDelay;
   }

   private void FixedUpdate()
   {
      if(canSpawn) {
         waveTimer += Time.fixedDeltaTime;
         if(waveTimer > waveDelay ) {
            waveTimer = 0f;
            SpawnWave();
         }
      }
   }

   private void SpawnWave()
   {
      if (waveContent.Length == 0) return;
      var curWave = waveContent[waveCounter];
      waveCounter++;
      waveCounter %= waveContent.Length;
      for( int i = 0; i < curWave.spawnPoints.Length; i++ ) {
         int j = i % curWave.enemies.Length;
         Instantiate(curWave.enemies[j], curWave.spawnPoints[i]);
      }
   }

   public void StartSpawn()
   {
      canSpawn = true;
   }

   public void StopSpawn()
   {
      canSpawn = false;
   }
}
