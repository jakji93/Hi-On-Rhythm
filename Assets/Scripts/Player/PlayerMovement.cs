using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Kamgam.SettingsGenerator;

public class PlayerMovement : MonoBehaviour, IHasProgress
{
   [SerializeField] private float playerMoveSpeed = 1f;
   [SerializeField] private float playerAcceleration = 1f;
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
   [SerializeField] private float playerDashSpeed = 25f;
   [SerializeField] private float dashCost = 30;
   [SerializeField] private float stamRecovery = 50;
   [SerializeField] private float dashTime = 2f;
   [SerializeField] private ParticleSystem dashParticle;
   [SerializeField] private Collider2D bodyCollider;

   [SerializeField] private InputActionAsset playerAction;
   // private Settings buttonSettings;

   private bool dashPress = false;
   private bool canMove = true;
   private Vector2 curVelopcity;

   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

   private void Start()
   {
      playerRB = GetComponent<Rigidbody2D>();
      originalScale = visualObject.localScale;
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
      if (canMove) 
        {
         if (playerMoveDir != Vector2.zero) 
         {
                //MovePlayer(playerMoveDir, playerMoveSpeed);
                if (dashPress)
                {
                    canMove = false;
                    StartCoroutine(dashRoutine(playerMoveDir));
                    dashPress = false;

                }
                else
                {
                    MovePlayer(playerMoveDir, playerMoveSpeed);
                    
                }

            } 
         else 
         {
                playerRB.velocity = Vector2.zero;
                curVelopcity = Vector2.zero;
         }
      }
   }

   void MovePlayer(Vector2 moveDir, float moveSpeed)
   {

        //playerRB.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
        playerRB.velocity = moveDir * moveSpeed;
        Vector2 targetVelocity = moveDir * moveSpeed;
        curVelopcity = Vector2.MoveTowards(curVelopcity, targetVelocity, playerAcceleration * Time.fixedDeltaTime);
      
   }

   void OnMove()
   {
       // playerAction.FindAction("Move").ReadValue<Vector2>();
        playerMoveDir = playerAction.FindAction("Move").ReadValue<Vector2>();

        //will change if using blender tree with 4 way animation
        var playerHasHorizontalSpeed = Mathf.Abs(playerMoveDir.x) > Mathf.Epsilon;
      if (playerHasHorizontalSpeed && playerMoveDir.x > 0) visualObject.localScale = new Vector2(originalScale.x * 1, originalScale.y * 1);
      else if (playerHasHorizontalSpeed && playerMoveDir.x < 0) visualObject.localScale = new Vector2(originalScale.x * -1, originalScale.y * 1);
      var playerHasSpeed = playerMoveDir.magnitude > Mathf.Epsilon;
      if (playerHasSpeed) animator.SetBool("isRun", true);
      else animator.SetBool("isRun", false);



   }

   void OnDodge(InputValue playerInput)
   {
      if (!GameplayManager.Instance.IsGamePlaying()) return;
      if (playerStam >= dashCost && playerMoveDir != Vector2.zero) {
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
      animator.SetBool("isDash", true);
      bodyCollider.enabled = false;
      while (curTime < dashTime) {
         MovePlayer( dashDir, playerDashSpeed);
         curTime = curTime + Time.deltaTime;
         yield return null;
      }
      
      canMove = true;
      animator.SetBool("isDash", false);
      bodyCollider.enabled = true;
      StopCoroutine("dashRoutine");
      curVelopcity = Vector2.zero;
      yield return null;
   }
}
