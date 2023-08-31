using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using MoreMountains.Tools;
using UnityEngine;

public class LoadCallback : MonoBehaviour
{
   [SerializeField] private bool useEasyTransition;
   [SerializeField] private TransitionSettings transition;
   private bool isFirstLoad = true;

   private void Update()
   {
      if(isFirstLoad) {
         isFirstLoad = false;
         StartCoroutine(StartLoading());
      }
   }

   private IEnumerator StartLoading()
   {
      yield return new WaitForSeconds(2.5f);
      if(useEasyTransition) {
         var target = Loader.GetTarget();
         TransitionManager.Instance().Transition(target, transition, 0f);
      } else {
         Loader.LoadCallback();
      }
   }

   private void OnDestroy()
   {
      StopAllCoroutines();
   }
}
