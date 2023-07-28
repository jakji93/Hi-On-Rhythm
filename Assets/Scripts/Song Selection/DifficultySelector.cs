using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
   [SerializeField] private Transform[] difficultyItemes;
   [SerializeField] private int spacing;
   [SerializeField] private AnimationCurve moveCurve;
   [SerializeField] private float moveSpeed = 10f;

   readonly private Difficulties[] difficulties = new Difficulties[] { Difficulties.Normal, Difficulties.Hard, Difficulties.Unbeatable };

   private int curSelected = 0;
   private Vector3 initPosition;
   private Vector3 targetPosition;
   private bool isMoving;

   private void Start()
   {
      for(int i = 0; i < difficultyItemes.Length; i++) {
         difficultyItemes[i].localPosition = new Vector3 (i* spacing, 0, 0);
      }
      LevelSelectManager.Instance.SetDifficulty(difficulties[curSelected]);
      isMoving = false;
   }

   public void Increase()
   {
      if (isMoving) return;
      if (curSelected == difficultyItemes.Length - 1) return;
      curSelected++;
      LevelSelectManager.Instance.SetDifficulty(difficulties[curSelected]);
      targetPosition = new Vector3(-curSelected * spacing, 0, 0);
      isMoving = true;
      transform.DOLocalMove(targetPosition, moveSpeed).SetEase(moveCurve).OnComplete(()=>
      {
         isMoving = false;
      });
   }

   public void Decrease()
   {
      if (isMoving) return;
      if (curSelected == 0) return;
      curSelected--;
      LevelSelectManager.Instance.SetDifficulty(difficulties[curSelected]);
      targetPosition = new Vector3(-curSelected * spacing, 0, 0);
      isMoving = true;
      transform.DOLocalMove(targetPosition, moveSpeed).SetEase(moveCurve).OnComplete(()=>
      {
         isMoving = false;
      });
   }

   private void LegacyMove()
   {
      //if (isMoving) {
      //   elapsedTime += Time.deltaTime;
      //   float normalizedTime = elapsedTime / moveSpeed;
      //   float curveValue = moveCurve.Evaluate(normalizedTime);

      //   transform.localPosition = Vector3.Lerp(initPosition, targetPosition, curveValue);

      //   if (normalizedTime >= 1f) {
      //      elapsedTime = 0f;
      //      transform.localPosition = targetPosition;
      //      initPosition = transform.localPosition;
      //      isMoving = false;
      //   }
      //}
   }
}
