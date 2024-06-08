using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Border : MonoBehaviour
{
   [SerializeField] private float duration;
   private Collider2D collider;
   private void Start()
   {
      collider = GetComponent<Collider2D>();
      collider.enabled = false;
      transform.DOMove(Vector3.zero, duration).From().OnComplete(() =>
      {
         collider.enabled = true;
      });
      transform.DOScale(0f, duration).From();
   }
}
