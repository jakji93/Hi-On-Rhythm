using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestingMove : MonoBehaviour
{
   [SerializeField] private Transform player;
   [SerializeField] private float moveSpeed = 1f;
   [SerializeField] private float rotateSpeed = 1f;

   private Vector2 preDirection;

   private void Start()
   {
      preDirection = (player.position - transform.position).normalized;
   }

   private void Update()
   {
      var newDirection = (player.position - transform.position).normalized;
      var slowDirction = Vector3.RotateTowards(preDirection, newDirection, Time.deltaTime * rotateSpeed, 0);
      preDirection = slowDirction;
      var distance = Vector2.Distance(player.position, transform.position);
      //transform.position = Vector2.MoveTowards(transform.position, transform.position + slowDirction*distance, moveSpeed * Time.deltaTime);
      transform.Translate(slowDirction*moveSpeed*Time.deltaTime);

   }

   private void OnDrawGizmos()
   {
      var newDirection = (player.position - transform.position).normalized;
      var slowDirction = Vector3.RotateTowards(preDirection, newDirection, Time.deltaTime * rotateSpeed, 0).normalized;
      Gizmos.color = Color.blue;
      Gizmos.DrawLine(transform.position, transform.position + slowDirction);
   }
}
