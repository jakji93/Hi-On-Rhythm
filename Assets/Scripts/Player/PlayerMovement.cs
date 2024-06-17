using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Kamgam.SettingsGenerator;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour, IHasProgress
{
   [SerializeField] private float playerMoveSpeed = 1f;
   private Vector2 playerMoveDir;
   private Rigidbody2D playerRB;

   [Header("Body")]
   [SerializeField] private Transform body;
   [SerializeField] private Health health;
   [Header("Dash")]
   [SerializeField] private float playerDashSpeed = 25f;
   [SerializeField] private float dashTime = 2f;
   [SerializeField] private Collider2D bodyCollider;
   [SerializeField] private GameObject dashAnime;
   [Header("Action")]
   [SerializeField] private InputActionAsset playerAction;

   private bool dashPress = false;
   private bool canMove = true;
   private bool canDodge = true;
   private bool isMoving = false;
   private bool isDead = false;

   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

   private void Start()
   {
      playerRB = GetComponent<Rigidbody2D>();
      health.OnDeath += Health_OnDeath;
      GameplayManager.Instance.OnStateChange += Instance_OnStateChange;
   }

   private void Instance_OnStateChange(object sender, EventArgs e)
   {
      if(GameplayManager.Instance.IsGameOver()) {
         canMove = false;
         canDodge = false;
         playerRB.velocity = Vector2.zero;
         playerMoveDir = Vector2.zero;
      }
   }

   private void Health_OnDeath(object sender, Health.OnTakeDamageEventArgs e)
   {
      canMove = false;
      canDodge = false;
      isDead = true;
      playerRB.velocity = Vector2.zero;
      playerMoveDir = Vector2.zero;
      body.localScale = Vector2.one;
   }

   private void FixedUpdate()
   {
      if (canMove) {
         if (playerMoveDir != Vector2.zero) {
            var normalizedDir = playerMoveDir.normalized;
            float angle = Mathf.Atan2(normalizedDir.y, normalizedDir.x) * Mathf.Rad2Deg;
            body.DORotate(new Vector3(0, 0, angle), 0.2f);
            if (dashPress && canDodge) {
               canMove = false;
               canDodge = false;
               StartCoroutine(dashRoutine(playerMoveDir));
               dashPress = false;
            }
            else {
               MovePlayer(playerMoveDir, playerMoveSpeed);

            }
         }
         else {
            playerRB.velocity = Vector2.zero;
         }
      }
   }

   void MovePlayer(Vector2 moveDir, float moveSpeed)
   {
      playerRB.velocity = moveDir * moveSpeed;
   }

   void OnMove()
   {
      if (isDead) return;
      playerMoveDir = playerAction.FindAction("Move").ReadValue<Vector2>();
      if(playerMoveDir != Vector2.zero && !isMoving) {
         isMoving = true;
         body.DOKill();
         body.DOScale(new Vector3(1.25f, 0.8f, 1), 0.2f);
      } else if(playerMoveDir == Vector2.zero && isMoving) {
         isMoving = false;
         body.DOKill();
         body.DOScale(new Vector3(1f, 1f, 1), 0.3f).SetEase(Ease.OutBounce);
      }
   }

   void OnDodge(InputValue playerInput)
   {
      if (playerMoveDir != Vector2.zero) {
         dashPress = true;
      } else {
         dashPress = false;
      }
   }

   private IEnumerator dashRoutine(Vector2 dashDir)
   {
      float curTime = 0;
      bodyCollider.enabled = false;
      Instantiate(dashAnime, transform.position, Quaternion.identity);
      while (curTime < dashTime) {
         MovePlayer( dashDir, playerDashSpeed);
         curTime = curTime + Time.deltaTime;
         yield return null;
      }
      canMove = true;
      bodyCollider.enabled = true;
      yield return new WaitForSeconds(0.2f);
      canDodge = true;
   }
}
