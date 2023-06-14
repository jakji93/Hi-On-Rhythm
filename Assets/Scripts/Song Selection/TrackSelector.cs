using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSelector : MonoBehaviour
{
   [SerializeField] private Transform[] trackItems;
   [SerializeField] private int spacing;
   [SerializeField] private AnimationCurve moveCurve;
   [SerializeField] private float moveSpeed = 10f;


   private int curSelected = 0;
   private Vector3 initPosition;
   private Vector3 targetPosition;
   private float elapsedTime;
   private bool isMoving = false;

   private void Start()
   {
      for (int i = 0; i < trackItems.Length; i++) {
         trackItems[i].localPosition = new Vector3(i * spacing, 0, 0);
      }
      initPosition = transform.localPosition;
      targetPosition = transform.localPosition;
      curSelected = LevelSelectManager.Instance.GetCurrentTrack();
      SetCurrentTrack(curSelected);
   }

   private void Update()
   {
      if (isMoving) {
         elapsedTime += Time.deltaTime;
         float normalizedTime = elapsedTime / moveSpeed;
         float curveValue = moveCurve.Evaluate(normalizedTime);

         transform.localPosition = Vector3.Lerp(initPosition, targetPosition, curveValue);

         if (normalizedTime >= 1f) {
            elapsedTime = 0f;
            transform.localPosition = targetPosition;
            initPosition = transform.localPosition;
            isMoving = false;
         }
      }
   }

   public void SetCurrentTrack(int index)
   {
      if (curSelected == index) return;
      curSelected = index;
      elapsedTime = 0;
      targetPosition = new Vector3(-curSelected * spacing, 0, 0);
      isMoving = true;
   }
}
