using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IHasProgress
{
   [SerializeField] private float playerMoveSpeed = 1f;
   [SerializeField] private float collisionOffset = 0.05f;
   [SerializeField] private ContactFilter2D movementFilter;
   [SerializeField] private Transform visualObject;
   [SerializeField] private Animator animator;
   private Vector3 originalScale;
   private Vector2 playerMoveDir;
   private Rigidbody2D playerRB;
   private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

   private float playerStam = 100f;
   [Header("Dash")]
   [SerializeField] private float playerDashSpeed = 10f;
   [SerializeField] private float dashCost = 30;
   [SerializeField] private float stamRecovery = 50;
   [SerializeField] private float dashTime = 2f;
   [SerializeField] private ParticleSystem particleSystem;
   [SerializeField] private Collider2D bodyCollider;
   private bool dashPress = false;
   private bool canMove = true;

   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

   private void Start()
   {
      playerRB = GetComponent<Rigidbody2D>();
      originalScale = visualObject.localScale;
      particleSystem.Stop();
   }

   private void Update()
   {
      if (playerStam < 100) {
         playerStam = Mathf.Min(playerStam + stamRecovery * Time.deltaTime, 100);
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = playerStam,
            maxValue = 100f
         });
      }
   }

   private void FixedUpdate()
   {
      if (canMove) {
         if (playerMoveDir != Vector2.zero) {

            int count = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerMoveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0 && !dashPress) {

               MovePlayer(playerRB.position, playerMoveDir, playerMoveSpeed);
            }

            else if (count == 0 && dashPress) {
               int count2 = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerDashSpeed * Time.fixedDeltaTime + collisionOffset);

               if (count2 == 0) {
                  //MovePlayer(playerRB.position, playerMoveDir, playerDashSpeed); dash
                  canMove = false;
                  StartCoroutine(dashRoutine(playerMoveDir));

                  dashPress = false;
               }
               else {

                  RaycastHit2D hitRay = Physics2D.Raycast(playerRB.position, playerMoveDir);

                  playerRB.MovePosition(playerRB.position + playerMoveDir * hitRay.fraction * .3f);


                  print(hitRay.fraction);
                  dashPress = false;

               }
            }
            else if (count != 0 && dashPress) {
               dashPress = false;
            }
         }
      }
   }

   void MovePlayer(Vector2 curPos, Vector2 moveDir, float moveSpeed)
   {
      playerRB.MovePosition(curPos + moveDir * moveSpeed * Time.fixedDeltaTime);
   }

   void OnMove(InputValue playerInput)
   {
      playerMoveDir = playerInput.Get<Vector2>().normalized;
      //will change if using blender tree with 4 way animation
      var playerHasHorizontalSpeed = Mathf.Abs(playerMoveDir.x) > Mathf.Epsilon;
      if (playerHasHorizontalSpeed && playerMoveDir.x > 0) visualObject.localScale = new Vector2(originalScale.x * 1, originalScale.y * 1);
      else if (playerHasHorizontalSpeed && playerMoveDir.x < 0) visualObject.localScale = new Vector2(originalScale.x * -1, originalScale.y * 1);
      var playerHasSpeed = playerMoveDir.magnitude > Mathf.Epsilon;
      if (playerHasSpeed) animator.SetBool("isRun", true);
      else animator.SetBool("isRun", false);
   }

   void OnSpace(InputValue playerInput)
   {
      if (playerStam >= dashCost) {
         dashPress = true;
         playerStam -= dashCost;
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = playerStam,
            maxValue = 100f
         });
      }
      else {
         dashPress = false;
      }

   }

   private IEnumerator dashRoutine(Vector2 dashDir)
   {
      float curTime = 0;
      particleSystem.Play();
      animator.SetBool("isDash", true);
      bodyCollider.enabled = false;
      while (curTime < dashTime) {
         MovePlayer(playerRB.position, dashDir, playerDashSpeed);
         curTime = curTime + Time.deltaTime;
         yield return null;
      }
      canMove = true;
      particleSystem.Stop();
      animator.SetBool("isDash", false);
      bodyCollider.enabled = true;
      StopCoroutine("dashRoutine");
      yield return null;
   }
}
