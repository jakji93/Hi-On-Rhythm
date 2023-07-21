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

   [System.Serializable]
   public class SpecificWaveContent
   {
      public int waveNumber;
      public WaveContent wave;
   }

   [SerializeField] private WaveContent[] waveContent;
   [SerializeField] private SpecificWaveContent[] specificWaveContent;
   [SerializeField] private float waveDelay = 1f;
   [SerializeField] private int totalWave = 0;

   [SerializeField] private bool canSpawn = false;

   private float waveTimer;
   private int waveCounter = 0;

   private int totalSpawned = 0;

   public static EnemySpawner Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      waveTimer = waveDelay;
   }

   private void Update()
   {
      if(canSpawn) {
         waveTimer += Time.deltaTime;
         if(waveTimer > waveDelay ) {
            waveTimer = 0f;
            SpawnWave();
         }
      }
   }

   private void SpawnWave()
   {
      if (waveContent.Length == 0) return;
      if (totalWave == 0) {
         StopSpawn();
         return;
      }
      var curWaveNum = waveCounter % waveContent.Length;
      var curWave = waveContent[curWaveNum];
      
      foreach( var wave in specificWaveContent ) {
         if(waveCounter+1 == wave.waveNumber ) {
            curWave = wave.wave;
         }
      }

      for( int i = 0; i < curWave.spawnPoints.Length; i++ ) {
         int j = i % curWave.enemies.Length;
         Instantiate(curWave.enemies[j], curWave.spawnPoints[i]);
         totalSpawned++;
      }
      waveCounter++;
      if (waveCounter >= totalWave) {
         StopSpawn();
         Debug.Log("Total Enemy Spawned: " +  totalSpawned);
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
