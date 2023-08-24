using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Border : MonoBehaviour
{
   [SerializeField] private float duration;
   private void Start()
   {
      transform.DOMove(Vector3.zero, duration).From();
      transform.DOScale(0f, duration).From();
   }
}
