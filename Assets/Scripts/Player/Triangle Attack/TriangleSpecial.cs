using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TriangleSpecial : MonoBehaviour
{
   [SerializeField] private float radius;
   [SerializeField] private float speed;
   [SerializeField] private float lifeTime;
   [SerializeField] private Transform projectile;

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, radius);
   }

   private void Start()
   {
      if (projectile == null) return;
      projectile.localPosition = new Vector3(radius, 0, 0);
      transform.DORotate(new Vector3(0, 0, 360), speed, RotateMode.FastBeyond360).SetSpeedBased(true).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
      if(lifeTime > 0) Destroy(gameObject, lifeTime);
   }

   private void OnDestroy()
   {
      transform.DOKill();
   }
}
