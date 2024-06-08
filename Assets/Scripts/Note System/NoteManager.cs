using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
   public static NoteManager Instance { get; private set; }

   public event EventHandler OnNormal1Hit;
   public event EventHandler OnNormal2Hit;
   public event EventHandler OnSpecialHit;
   public event EventHandler OnNotePerfect;
   public event EventHandler OnNoteGreat;
   public event EventHandler OnNoteGood;
   public event EventHandler OnNoteMissed;
   public event EventHandler OnWrongNote;
   public event EventHandler OnNoNoteHits;
   public event EventHandler OnAttackBeat;
   public event EventHandler OnSpawnBeat;

   [SerializeField] private Vector2 hitzoneSize;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private LayerMask noteLayer;
   [Header("SFX")]
   [SerializeField] private AudioClip noteHitSound;
   [Header("Note Accuracy")]
   [Tooltip("Accuracy zone in px")]
   [SerializeField] private int perfectZone;
   [SerializeField] private int greatZone;
   [SerializeField] private int goodZone;
   [SerializeField] private int maxHitdistance;
   [SerializeField] private TextMeshProUGUI accuracyNum;
   [SerializeField] private TextMeshProUGUI lateJudgeText;
   [SerializeField] private bool autoplay = false;

   private float totalAccuracy = 0f;
   private float currentAccuracy = 0f;
   private float accuracy = 0f;
   private int early = 0;
   private int late = 0;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      gameInput.OnNormal1Pressed += GameInput_OnNormal1Pressed;
      gameInput.OnNormal2Pressed += GameInput_OnNormal2Pressed;
      accuracyNum.text = "0.00%";
      lateJudgeText.text = "-";
   }

   private void GameInput_OnNormal1Pressed(object sender, System.EventArgs e)
   {
      DetectNoteHit();
   }

   private void GameInput_OnNormal2Pressed(object sender, System.EventArgs e)
   {
      DetectNoteHit();
   }

   private void DetectNoteHit()
   {
      if (autoplay) return;
      if (!GameplayManager.Instance.IsGamePlaying()) return;
      var noteObj = Physics2D.OverlapBox(transform.position, hitzoneSize, noteLayer);
      if (noteObj != null) {
         var notePosition = noteObj.transform.position;
         noteObj.gameObject.TryGetComponent(out Note note);
         switch (note.getNoteType()) {
            case Note.NoteTypes.Normal1:
               OnNormal1Hit?.Invoke(this, EventArgs.Empty);
               ClipPlayer.Instance.PlayClip(noteHitSound);
               Debug.Log("Attack hit");
               break;
            case Note.NoteTypes.Normal2:
               OnNormal1Hit?.Invoke(this, EventArgs.Empty);
               ClipPlayer.Instance.PlayClip(noteHitSound);
               Debug.Log("Attack hit");
               break;
            case Note.NoteTypes.Special:
               OnSpecialHit?.Invoke(this, EventArgs.Empty);
               ClipPlayer.Instance.PlayClip(noteHitSound);
               Debug.Log("Special hit");
               break;
         }
         CalculateAccuracy(notePosition);
         if (noteObj.gameObject.TryGetComponent(out OsuMarker marker)) {
            marker.DestroyCircle();
         }
         note.gameObject.SetActive(false);
      }
      else {
         Debug.Log("No hit");
         OnNoNoteHits?.Invoke(this, EventArgs.Empty);
      }
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(transform.position, hitzoneSize);
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if ((noteLayer.value & (1 << collision.transform.gameObject.layer)) > 0) {
         if (collision.TryGetComponent(out Note note)) {
            var noteType = note.getNoteType();
            switch (noteType) {
               case Note.NoteTypes.Attack:
                  OnAttackBeat?.Invoke(this, EventArgs.Empty);
                  break;
               case Note.NoteTypes.Spawn:
                  OnSpawnBeat?.Invoke(this, EventArgs.Empty);
                  Debug.Log("Enemy Spawn");
                  break;
               case Note.NoteTypes.Normal1:
                  if (autoplay) OnNotePerfect?.Invoke(this, EventArgs.Empty);
                  else OnNoteMissed?.Invoke(this, EventArgs.Empty);
                  totalAccuracy += 300;
                  accuracy = Mathf.Round(currentAccuracy / totalAccuracy * 100 * 100) / 100;
                  accuracyNum.text = accuracy.ToString() + "%";
                  if (note.gameObject.TryGetComponent(out OsuMarker marker)) {
                     marker.DestroyCircle();
                  }
                  note.gameObject.SetActive(false);
                  break;
               default: break;
            }
         }
      }
   }

   private void CalculateAccuracy(Vector3 notePosition)
   {
      var distance = Vector2.Distance(transform.position, notePosition);
      if(distance <= perfectZone) {
         OnNotePerfect?.Invoke(this, EventArgs.Empty);
         currentAccuracy += 300;
      } else if(distance <= greatZone) {
         OnNoteGreat?.Invoke(this, EventArgs.Empty);
         currentAccuracy += 100;
      } else if (distance <= goodZone) {
         OnNoteGood?.Invoke(this, EventArgs.Empty);
         currentAccuracy += 50;
      }
      totalAccuracy += 300;
      accuracy = Mathf.Round(currentAccuracy / totalAccuracy * 100 * 100) / 100;
      var latejudge = notePosition.x - transform.position.x;
      if (latejudge < 0) {
         late++;
      } else if (latejudge > 0) {
         early++;
      }
      accuracyNum.text = accuracy.ToString() + "%";
      if(early > late) {
         lateJudgeText.text = "Mostly early";
      } else {
         lateJudgeText.text = "Mostly late";
      }
   }

   public float GetAccuracyRating()
   {
      return currentAccuracy / totalAccuracy;
   }

   private void OnDestroy()
   {
      gameInput.OnNormal1Pressed -= GameInput_OnNormal1Pressed;
      gameInput.OnNormal2Pressed -= GameInput_OnNormal2Pressed;
   }
}
