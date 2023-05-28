using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
   [SerializeField] private float rotationSpeed = 60f; // Speed of rotation in degrees per second
   [SerializeField] private bool clockWise = true;

   private RectTransform rectTransform;
   private Coroutine rotationCoroutine;
   private int rotateDirection = -1;

   private void Start()
   {
      rectTransform = GetComponent<RectTransform>();
      rotateDirection = clockWise ? -1 : 1;
      StartRotationCoroutine();
   }

   private void OnDestroy()
   {
      StopRotationCoroutine();
   }

   private void StartRotationCoroutine()
   {
      if (rotationCoroutine == null) {
         rotationCoroutine = StartCoroutine(RotateUI());
      }
   }

   private void StopRotationCoroutine()
   {
      if (rotationCoroutine != null) {
         StopCoroutine(rotationCoroutine);
         rotationCoroutine = null;
      }
   }

   private IEnumerator RotateUI()
   {
      while (true) {
         rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime * rotateDirection);
         yield return null;
      }
   }
}
