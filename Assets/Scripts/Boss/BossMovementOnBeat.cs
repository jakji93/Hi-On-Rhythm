using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kamgam.SettingsGenerator;
using UnityEngine;
using static EnemySpawner;

public class BossMovementOnBeat : MonoBehaviour
{

   [SerializeField] private Transform startingPosition;

   [Serializable]
   public class BossMoves
   {
      public Transform position;
      public Ease ease;
      public bool useAnimationCurve;
      public AnimationCurve curve;
      public float duration;
      public bool isSpeedBased = false;
   }

   [Serializable]
   public class BossMovePattern
   {
      public BossMoves[] positions;
      public float musicTime;
   }

   [Serializable]
   public class SpecificBossMovePattern
   {
      public BossMoves move;
      public int waveNumber;
   }

   [SerializeField] private BossMovePattern[] movePattern;
   [SerializeField] private SpecificBossMovePattern[] specificMovePattern;

   private int waveCounter = 0;
   private int globalWaveCounter = 0;
   private BossMovePattern curPattern;

   private void Start()
   {
      NoteManager.Instance.OnSpawnBeat += NoteManager_OnSpawnBeat;
      transform.DOMove(startingPosition.position, 2).From().SetEase(Ease.OutBounce);
      if(!movePattern.IsNullOrEmpty()) curPattern = movePattern[waveCounter];
   }

   private void NoteManager_OnSpawnBeat(object sender, EventArgs e)
   {
      UpdateCurSet();
      Move();
   }

   private void UpdateCurSet()
   {
      if (curPattern == null) return;
      var playtime = MusicManager.Instance.GetGameMusicPlaytime();
      if (playtime > curPattern.musicTime) {
         foreach (var set in movePattern) {
            if (playtime < set.musicTime) {
               curPattern = set;
               waveCounter = 0;
               return;
            }
         }
         curPattern = movePattern[movePattern.Length - 1];
      }
   }

   private void Move()
   {
      if (curPattern == null) return;
      var curWaveNum = waveCounter % curPattern.positions.Length;
      var curMove = curPattern.positions[curWaveNum];

      foreach (var pattern in specificMovePattern) {
         if (globalWaveCounter + 1 == pattern.waveNumber) {
            curMove = pattern.move;
         }
      }

      transform.DOKill();
      if(curMove.useAnimationCurve) {
         transform.DOMove(curMove.position.position, curMove.duration).SetEase(curMove.curve).SetSpeedBased(curMove.isSpeedBased);
      } else {
         transform.DOMove(curMove.position.position, curMove.duration).SetEase(curMove.ease).SetSpeedBased(curMove.isSpeedBased);
      }

      waveCounter++;
      globalWaveCounter++;
   }
}
