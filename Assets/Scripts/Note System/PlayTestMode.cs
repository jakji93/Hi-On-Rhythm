using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class PlayTestMode : MonoBehaviour
{
   [SerializeField] private Vector2 hitzoneSize;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private LayerMask noteLayer;
   [SerializeField] private bool isTestMode;

   private void Start()
   {
      gameInput.OnNormal1Pressed += GameInput_OnNormal1Pressed;
      gameInput.OnNormal2Pressed += GameInput_OnNormal2Pressed;
   }

   private void GameInput_OnNormal1Pressed(object sender, System.EventArgs e)
   {
      if (!isTestMode) return;
      var noteObj = Physics2D.OverlapBox(transform.position, hitzoneSize, noteLayer);
      if (noteObj != null) {
         noteObj.gameObject.SetActive(false);
      }
   }

   private void GameInput_OnNormal2Pressed(object sender, System.EventArgs e)
   {
      if (!isTestMode) return;
      var noteObj = Physics2D.OverlapBox(transform.position, hitzoneSize, noteLayer);
      if (noteObj != null) {
         noteObj.gameObject.SetActive(false);
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
         if(collision.TryGetComponent(out Note note)) {
            var noteType = note.getNoteType();
            switch (noteType) {
               case Note.NoteTypes.Attack:
                  Debug.Log("Attack");
                  break;
               case Note.NoteTypes.Spawn:
                  Debug.Log("Spawn");
                  break;
            }
         }
      }
   }
}
