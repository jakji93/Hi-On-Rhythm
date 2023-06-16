using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCallback : MonoBehaviour
{
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
      yield return new WaitForSeconds(2f);
      Loader.LoadCallback();
   }

   private void OnDestroy()
   {
      StopAllCoroutines();
   }
}
