using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;



public class Attack1 : MonoBehaviour
{
   [SerializeField] private Vector2 attackSize;
   [SerializeField] private Transform attackPoint;
   [SerializeField] private LayerMask attackLayerMask;
   [SerializeField] private int damage = 10;

   private void Awake()
   {
      transform.parent = PlayerControl.Instance.transform;
   }
   private void Start()
   {
      var colliders = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0, attackLayerMask);
      if (colliders == null) return;
      foreach (var collider in colliders) {
         if (collider.gameObject.TryGetComponent(out Health health)) {
            health.TakeDamage(damage * ComboManager.Instance.GetMultiplier());
         }
      }
      Destroy(gameObject, 2f);
   }

   private void OnDrawGizmos()
   {
      if(attackPoint != null) {
         Gizmos.color = Color.red;
         Gizmos.DrawWireCube(attackPoint.position, attackSize);
      }
   }
}
