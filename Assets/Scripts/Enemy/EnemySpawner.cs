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
      public float musicTime;
   }

   [SerializeField] private AdvancedWaveContent[] advancedWaveContents;
   [SerializeField] private SpecificWaveContent[] specificWaveContents;
   [SerializeField] private GameObject summonAnimation;
   [SerializeField] private bool canSpawn = false;

   private int waveCounter = 0;
   private int globalWaveCounter = 0;
   private AdvancedWaveContent curSet;
   private bool useEffect = true;

   private int totalSpawned = 0;

   public static EnemySpawner Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NoteManager.Instance.OnSpawnBeat += NoteManager_OnSpawnBeat;
      useEffect = GameplayManager.Instance.UseEffect();
   }

   private void NoteManager_OnSpawnBeat(object sender, System.EventArgs e)
   {
      if(canSpawn) {
         UpdateCurSet();
         SpawnWave();
      }
   }

   private void Update()
   {
   }

   private void SpawnWave()
   {
      var curWaveNum = waveCounter % curSet.wavesContents.Length;
      var curWave = curSet.wavesContents[curWaveNum];
      
      foreach( var wave in specificWaveContents) {
         if(globalWaveCounter + 1 == wave.waveNumber ) {
            curWave = wave.wave;
         }
      }

      for( int i = 0; i < curWave.spawnPoints.Length; i++ ) {
         if (curWave.enemies.Length == 0) break;
         int j = i % curWave.enemies.Length;
         Instantiate(curWave.enemies[j], curWave.spawnPoints[i].position, curWave.enemies[j].transform.rotation);
         if (useEffect) Instantiate(summonAnimation, curWave.spawnPoints[i].position, Quaternion.identity);
         totalSpawned++;
      }
      waveCounter++;
      globalWaveCounter++;
   }

   private void UpdateCurSet()
   {
      var playtime = MusicManager.Instance.GetGameMusicPlaytime();
      if(playtime > curSet.musicTime) {
         foreach(var set in advancedWaveContents ) {
            if(playtime < set.musicTime) {
               curSet = set;
               waveCounter = 0;
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
      globalWaveCounter = 0;
   }

   public void StopSpawn()
   {
      canSpawn = false;
   }

   public int GetTotalSpawned()
   {
      Debug.Log(totalSpawned);
      return totalSpawned;
   }
}
