using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyHit : MonoBehaviour
{
   [SerializeField] private int damage;
   [SerializeField] private Rigidbody2D playerRB;
   private bool applyDamage = false;
   [SerializeField] private float playerLoseControlTime = 0.5f;
   [SerializeField] private PlayerMovement playermove;
   [SerializeField] private float forceApply = 40f;
   [SerializeField] private LayerMask targetMask;
   [SerializeField] private Health playerHealth;
   [SerializeField] private GameObject explosion;
   [SerializeField] private AudioClip hitAudio;

   private float controlTimer = -1;
   private bool alreadyHit = false;

   private void Start()
   {
      GameplayManager.Instance.OnStateChange += Instance_OnStateChange;
   }

   private void Update()
   {
      if(controlTimer < 0) {
         playermove.enabled = true;
         alreadyHit = false;
      } else {
         controlTimer -= Time.deltaTime;
      }
   }

   private void Instance_OnStateChange(object sender, System.EventArgs e)
   {
      if(GameplayManager.Instance.IsGamePlaying()) {
         applyDamage = true;
      } else {
         applyDamage = false;
      }
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (((1 << collision.gameObject.layer) & targetMask) != 0) {
         if(applyDamage) playerHealth.TakeDamage(damage);
      }
   }

   private void OnCollisionStay2D(Collision2D collision)
   {
      if (alreadyHit) return;
      if (((1 << collision.gameObject.layer) & targetMask) != 0) {
         playermove.enabled = false;
         controlTimer = playerLoseControlTime;
         var playerDir = (PlayerControl.Instance.transform.position - transform.position).normalized;
         if (explosion != null) {
            var exp = Instantiate(explosion, PlayerControl.Instance.transform.position, Quaternion.identity);
            Destroy(exp, 2f);
         }
         playerRB.AddForce(playerDir * forceApply, ForceMode2D.Impulse);
         alreadyHit = true;
      }
   }
}
