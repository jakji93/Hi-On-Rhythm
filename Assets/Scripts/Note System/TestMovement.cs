using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
   [SerializeField] float noteSpeed = 3600f;
   private float timeSofar = 0f;
   private void Start()
   {
      Debug.Log(transform.localPosition.x);
   }

   private void FixedUpdate()
   {
      timeSofar += Time.fixedDeltaTime;
      transform.position -= new Vector3(noteSpeed * Time.fixedDeltaTime, 0, 0);
   }
}
