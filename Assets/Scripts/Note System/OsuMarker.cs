using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuMarker : MonoBehaviour
{
   [SerializeField] private Transform circlePrefab;
   [SerializeField] private float warningInBeats = 3f;
   [SerializeField] private float minScale = 1f;
   [SerializeField] private float maxScale = 3f;
   [SerializeField] private float baseScale = 1f;
   [SerializeField] private AnimationCurve alphaCurve;

   private Transform noteHitZone;
   private Transform player;
   private Transform thisCircle;
   private SpriteRenderer circleSprite;

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
         circleSprite = thisCircle.GetComponent<SpriteRenderer>();
      }
      if (thisCircle != null) {
         var curScale = baseScale + (maxScale - baseScale) * (distance / (480 * warningInBeats));
         var scale = Mathf.Max(curScale, minScale);
         thisCircle.localScale = new Vector3(scale, scale, 0);
      }
      if(circleSprite != null) {
         var normalizeDistance = (1 - distance / (480 * warningInBeats));
         var alphaScale = alphaCurve.Evaluate(normalizeDistance);
         alphaScale = Mathf.Clamp(alphaScale, 0f, 1f);
         var color = circleSprite.color;
         color.a = alphaScale;
         circleSprite.color = color;
      }
   }

   public void DestroyCircle()
   {
      if (thisCircle != null) {
         circleSprite = null;
         Destroy(thisCircle.gameObject);
      }
   }
}
