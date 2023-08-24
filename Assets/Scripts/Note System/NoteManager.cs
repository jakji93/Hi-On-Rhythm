using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
   public static NoteManager Instance { get; private set; }

   public event EventHandler OnNormal1Hit;
   public event EventHandler OnNormal2Hit;
   public event EventHandler OnSpecialHit;
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

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      gameInput.OnNormal1Pressed += GameInput_OnNormal1Pressed;
      gameInput.OnNormal2Pressed += GameInput_OnNormal2Pressed;
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
      if (!GameplayManager.Instance.IsGamePlaying()) return;
      var noteObj = Physics2D.OverlapBox(transform.position, hitzoneSize, noteLayer);
      if (noteObj != null) {
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
                  Debug.Log("Enemy Attack");
                  break;
               case Note.NoteTypes.Spawn:
                  OnSpawnBeat?.Invoke(this, EventArgs.Empty);
                  Debug.Log("Enemy Spawn");
                  break;
               case Note.NoteTypes.Normal1:
                  OnNoteMissed?.Invoke(this, EventArgs.Empty);
                  Debug.Log("Note missed");
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
}
