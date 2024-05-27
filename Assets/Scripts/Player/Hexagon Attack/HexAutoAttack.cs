using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HexAutoAttack : MonoBehaviour
{
   [SerializeField] private Transform innerHex;
   [SerializeField] private Collider2D hitCollider;
   [SerializeField] private ContactFilter2D contactFilter;
   [SerializeField] private int damage;
   [SerializeField] private GameObject explodeParticle;

   private List<Collider2D> colliders = new List<Collider2D>();

   private void Start()
   {
      innerHex.DOScale(0f, 0.5f).From().OnComplete(() =>
      {
         var expParticle = Instantiate(explodeParticle, transform.position, Quaternion.identity);
         Destroy(expParticle, 1f);
         Attack();
         Destroy(gameObject);
      });
   }

   private void Attack()
   {
      hitCollider.OverlapCollider(contactFilter, colliders);
      foreach (var collider in colliders) {
         if (collider.gameObject.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
         }
      }
   }
}
