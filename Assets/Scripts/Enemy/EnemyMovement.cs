using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
   [SerializeField] private Transform player;
   [SerializeField] private EnemyController controller;
   [SerializeField] private LayerMask enemyMask;

   [SerializeField] private bool canMove = true;
   [SerializeField] private float moveSpeed = 1f;
   [SerializeField] private float spaceBetween = 0f;
   [SerializeField] private float turnSpeed = 2f;

   private float baseScaleX;
   private Vector2 preDirection;

   private void Awake()
   {
      baseScaleX = transform.localScale.x;
   }

   private void Start()
   {
      controller.OnStateChanged += EnemyController_OnStateChanged;
      player = GameObject.FindGameObjectWithTag("Player").transform;
      preDirection = (player.position - transform.position).normalized;
   }

   private void EnemyController_OnStateChanged(object sender, EnemyController.OnStateChangedEventArgs e)
   {
      canMove = e.state == EnemyController.EnemyState.Move;
   }

   private void Update()
   {
      if (canMove) {
         Flock();
         Move();
         FaceTarget();
      }
   }

   private void FaceTarget()
   {
      var scale = transform.localScale;
      scale.x = transform.position.x > player.position.x ? -baseScaleX : baseScaleX;
      transform.localScale = scale;
   }

   private void Flock()
   {
      var enemies = Physics2D.OverlapCircleAll(transform.position, spaceBetween, enemyMask);
      foreach (var enemy in enemies) {
         if (gameObject != enemy.gameObject) {
            Vector2 direction = transform.position - enemy.transform.position;
            direction.Normalize();
            transform.Translate(direction * moveSpeed * Time.deltaTime);
         }
      }
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.cyan;
      Gizmos.DrawWireSphere(transform.position, spaceBetween);
   }

   private void Move()
   {
      //transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
      var newDirection = (player.position - transform.position).normalized;
      var slowDirction = Vector3.RotateTowards(preDirection, newDirection, Time.deltaTime * turnSpeed, 0);
      preDirection = slowDirction;
      transform.Translate(moveSpeed * Time.deltaTime * slowDirction);
   }
}
