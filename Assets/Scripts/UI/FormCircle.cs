using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FormCircle : MonoBehaviour
{
   [SerializeField] private float radius = 200f;
   [SerializeField] private float speed = 10f;
   private void Start()
   {
      var angle = 360f / transform.childCount;
      for (int i = 0; i < transform.childCount; i++) {
         float yPos = Mathf.Sin(Mathf.Deg2Rad * (i * angle)) * radius;
         float xPos = -Mathf.Cos(Mathf.Deg2Rad * (i * angle)) * radius;
         transform.GetChild(i).localPosition = new Vector3(xPos, yPos, 0f);
         transform.GetChild(i).Rotate(new Vector3(0f, 0f, -i * angle + 90));
      }
      transform.DORotate(new Vector3(0f, 0f, 360f), speed, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetRelative();
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(transform.position, radius);
   }
}
