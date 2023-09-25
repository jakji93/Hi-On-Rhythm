using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ExplosionAttack : MonoBehaviour
{
   [SerializeField] private EnemyController controller;
   [SerializeField] private Transform body;
   [SerializeField] private int damage;
   [SerializeField] private Transform outterRing;
   [SerializeField] private Transform innerRing;
   [SerializeField] private GameObject explodeParticle;
   [SerializeField] private Collider2D attackCollider;
   [SerializeField] private ContactFilter2D contactFilter2D;
   [SerializeField] private float moveSpeed;

   private Transform player;

   private List<Collider2D> colliders = new List<Collider2D>();

   protected bool isAttacking = false;
   private bool canAttack = false;

   private void Start()
   {
      controller.OnStateChanged += EnemyController_OnStateChanged;
      player = GameObject.FindGameObjectWithTag("Player").transform;
   }

   private void EnemyController_OnStateChanged(object sender, EnemyController.OnStateChangedEventArgs e)
   {
      if (!GameplayManager.Instance.IsGamePlaying()) return;
      if (e.state == EnemyController.EnemyState.Attack) {
         canAttack = true;
         controller.IsAttacking(true);
         body.DOShakePosition(2f, 0.3f, 100, 90f, false, false);
         outterRing.gameObject.SetActive(true);
         innerRing.DOScale(0f, 1f).From().OnComplete(() =>
         {
            var expParticle = Instantiate(explodeParticle, body.position, Quaternion.identity);
            Attack();
            Destroy(expParticle, 1f);
            Destroy(gameObject);
         });
      }
   }

   private void Update()
   {
      if (canAttack) {
         Move();
      }
   }

   private void OnDestroy()
   {
      body.DOKill();
      innerRing.DOKill();
   }

   private void Attack()
   {
      attackCollider.OverlapCollider(contactFilter2D, colliders);
      foreach (var collider in colliders) {
         if (collider.gameObject.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
         }
      }
   }

   private void Move()
   {
      var newDirection = (player.position - transform.position).normalized;
      transform.Translate(moveSpeed * Time.deltaTime * newDirection);
   }

}
