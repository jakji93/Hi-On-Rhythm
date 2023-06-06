using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IHasProgress
{
   [SerializeField] private float playerMoveSpeed = 1f;
   [SerializeField] private float collisionOffset = 0.05f;
   [SerializeField] private ContactFilter2D movementFilter;
   private Vector2 playerMoveDir;
   private Rigidbody2D playerRB;
   private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

   private float playerStam = 100f;
   [SerializeField] private float playerDashSpeed = 10f;
   private bool dashPress = false;

   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

   private void Start()
   {
      playerRB = GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
      if (playerStam < 100) {
         playerStam++;
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = playerStam,
            maxValue = 100f
         });
      }
   }

   private void FixedUpdate()
   {
      if (playerMoveDir != Vector2.zero) {

         int count = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerMoveSpeed * Time.fixedDeltaTime + collisionOffset);

         if (count == 0 && !dashPress) {

            MovePlayer(playerRB.position, playerMoveDir, playerMoveSpeed);
         }

         else if (count == 0 && dashPress) {
            int count2 = playerRB.Cast(playerMoveDir, movementFilter, castCollisions, playerDashSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count2 == 0) {
               MovePlayer(playerRB.position, playerMoveDir, playerDashSpeed);
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
            playerStam = 0;
            print("wall");
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

   }

   void OnSpace(InputValue playerInput)
   {
      if (playerStam == 100) {
         dashPress = true;
         playerStam = 0;
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
}
