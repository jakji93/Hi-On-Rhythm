using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeSyncer : MonoBehaviour
{
   [SerializeField] protected float bias;
   [SerializeField] protected float timeStep;
   [SerializeField] protected float timeToBeat;
   [SerializeField] protected float restSmoothTime;

   private float previousSyncValue;
   private float curSyncValue;
   private float timer;

   protected bool isBeat;

   public virtual void OnBeat()
   {
      timer = 0;
      isBeat = true;
   }

   public virtual void OnUpdate()
   {
      previousSyncValue = curSyncValue;
      curSyncValue = MusicManager.Instance.GetAmplitude() * 100;

      if (previousSyncValue <= bias && curSyncValue > bias) {
         if (timer > timeStep) {
            OnBeat();
         }
      }
      timer += Time.deltaTime;
   }

   private void Update()
   {
      OnUpdate();
   }
}
