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

   [SerializeField] private Vector2 hitzoneSize;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private LayerMask noteLayer;
   [SerializeField] private Vector2 missZoneSize;
   [SerializeField] private Transform missZoneTransform;

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
      var note = Physics2D.OverlapBox(transform.position, hitzoneSize, noteLayer);
      if(note != null) {
         note.gameObject.TryGetComponent(out NoteType noteType);
         switch(noteType.getNoteType()) {
            case NoteType.NoteTypes.Normal1:
               OnNormal1Hit?.Invoke(this, EventArgs.Empty);
               Debug.Log("Normal 1 hit");
               break;
            case NoteType.NoteTypes.Normal2:
               OnNoteMissed?.Invoke(this, EventArgs.Empty);
               Debug.Log("Hit wrong note");
               break;
            case NoteType.NoteTypes.Special:
               OnSpecialHit?.Invoke(this, EventArgs.Empty);
               Debug.Log("Special hit");
               break;
         }
         //play note hit animation
         note.gameObject.SetActive(false);
      } else {
         Debug.Log("No hit");
         OnNoteMissed?.Invoke(this, EventArgs.Empty);
      }
   }

   private void GameInput_OnNormal2Pressed(object sender, System.EventArgs e)
   {
      var note = Physics2D.OverlapBox(transform.position, hitzoneSize, noteLayer);
      if (note != null) {
         note.gameObject.TryGetComponent(out NoteType noteType);
         switch (noteType.getNoteType()) {
            case NoteType.NoteTypes.Normal1:
               OnNoteMissed?.Invoke(this, EventArgs.Empty);
               Debug.Log("Hit wrong note");
               break;
            case NoteType.NoteTypes.Normal2:
               OnNormal2Hit?.Invoke(this, EventArgs.Empty);
               Debug.Log("Normal 2 hit");
               break;
            case NoteType.NoteTypes.Special:
               OnSpecialHit?.Invoke(this, EventArgs.Empty);
               Debug.Log("Special hit");
               break;
         }
         //play note hit animation
         note.gameObject.SetActive(false);
      }
      else {
         Debug.Log("No hit");
         OnNoteMissed?.Invoke(this, EventArgs.Empty);
      }
   }

   private void Update()
   {
      var note = Physics2D.OverlapBox(missZoneTransform.position, missZoneSize, noteLayer);
      if (note != null) {
         OnNoteMissed?.Invoke(this, EventArgs.Empty);
         Debug.Log("Note missed");
         //play note hit animation
         note.gameObject.SetActive(false);
      }
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(transform.position, hitzoneSize);

      Gizmos.color = Color.cyan;
      Gizmos.DrawWireCube(missZoneTransform.position, missZoneSize);
   }
}
