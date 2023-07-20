using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuMarker : MonoBehaviour
{
   [SerializeField] private Transform circlePrefab;
   [SerializeField] private float warningInBeats = 3f;
   [SerializeField] private float minScale = 1f;
   [SerializeField] private float baseScale = 1f;

   private Transform noteHitZone;
   private Transform player;
   private Transform thisCircle;

   private void Start()
   {
      player = PlayerControl.Instance.gameObject.transform;
      noteHitZone = NoteManager.Instance.gameObject.transform;
   }

   private void Update()
   {
      var distance = transform.position.x - noteHitZone.position.x;

      if (distance < (480 * warningInBeats) && thisCircle == null) {
         thisCircle = Instantiate(circlePrefab, player);
      }
      if (thisCircle != null) {
         var scale = Mathf.Max(baseScale + distance / 480, minScale);
         thisCircle.localScale = new Vector3(scale, scale, 0);
      }
   }

   public void DestroyCircle()
   {
      if (thisCircle != null) {
         Destroy(thisCircle.gameObject);
      }
   }
}
