using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTrail : MonoBehaviour
{
   private Transform thisTransform;

   private void Start()
   {
      thisTransform = transform;
      var target = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
      thisTransform.position = (Vector2)target;
      if (PlayerPrefs.HasKey("SpecialEffects")) {
         gameObject.SetActive(PlayerPrefs.GetInt("SpecialEffects") == 1);
      }
   }

   private void Update()
   {
      var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      thisTransform.position = (Vector2)target;
   }
}
