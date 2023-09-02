using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrainAnimation : MonoBehaviour
{
   private void Start()
   {
      transform.DOMoveX(40f, 20f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
   }
}
