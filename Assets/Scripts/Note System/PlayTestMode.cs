using System;
using System.Collections;
using System.Collections.Generic;
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
}
