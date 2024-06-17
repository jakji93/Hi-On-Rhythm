using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boss1Attack5 : MonoBehaviour
{
   [SerializeField] private Transform attackZone;
   [SerializeField] private int damage;
   [SerializeField] private Collider2D attackCollider;
   [SerializeField] private ContactFilter2D contactFilter;
   [Min(0)]
   [SerializeField] private float delay;
   [SerializeField] private float maintainTime = -1f;

   private List<Collider2D> colliders = new List<Collider2D>();
   private bool startMaintain = false;
   private float maintainTimer = 0f;

   private void Start()
   {
      attackZone.DOScaleX(0, delay).From().SetEase(Ease.Linear).OnComplete(() =>
      {
         Attack();
         startMaintain = true;
         maintainTimer = maintainTime;
      });
   }

   private void Update()
   {
      if(startMaintain) {
         if(maintainTimer < 0) {
            Destroy(gameObject);
         } else {
            maintainTimer -= Time.deltaTime;
            Attack();
         }
      }
   }

   private void Attack()
   {
      attackCollider.OverlapCollider(contactFilter, colliders);
      foreach (var collider in colliders) {
         if (collider.gameObject.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
         }
      }
   }
}
