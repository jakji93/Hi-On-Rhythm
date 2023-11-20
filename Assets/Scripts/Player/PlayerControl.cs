using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
   public static PlayerControl Instance { get; private set; }  
   [SerializeField] private Health health;
   [SerializeField] private TextMeshProUGUI healthText;
   [SerializeField] private AudioClip hitSFX;
   [SerializeField] private AudioClip missedSFX;
   [SerializeField] private SpriteRenderer bodySprite;
   [SerializeField] private Color missColor;
   [SerializeField] private Color defaultColor;

   private void Awake()
   {
      Instance = this;
   }

   void Start()
   {
      health.OnDeath += Health_OnDeath;
      health.OnTakeDamage += Health_OnTakeDamage;
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnNoNoteHits += NoteManager_OnNoNoteHits;
      NoteManager.Instance.OnWrongNote += NoteManager_OnWrongNote;
      healthText.text = health.GetMaxHealth().ToString();
      bodySprite.color = defaultColor;
   }

   private void NoteManager_OnWrongNote(object sender, System.EventArgs e)
   {
      //Might take this away too if is too hard
      //health.TakeDamage(damageOnWrongNote);
   }

   private void Health_OnTakeDamage(object sender, Health.OnTakeDamageEventArgs e)
   {
      healthText.text = health.GetCurHealth().ToString();
      HitFlashManager.Instance.Flash();
      if(hitSFX != null) ClipPlayer.Instance.PlayClip(hitSFX);
      //Note: maybe lowering multiplier on hit is too much?
      //ComboManager.Instance?.GotHit();
   }

   private void NoteManager_OnNoNoteHits(object sender, System.EventArgs e)
   {
      //health.TakeDamage(damageOnNoNoteHit);
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      if (missedSFX != null) ClipPlayer.Instance.PlayClip(missedSFX);
      MissFlashManager.Instance.Flash();
      bodySprite.DOColor(missColor, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
      {
         bodySprite.DOColor(defaultColor, 0.1f).SetEase(Ease.Linear);
      });
   }

   private void Health_OnDeath(object sender, Health.OnTakeDamageEventArgs e)
   {
      //trigger player dead animation
      healthText.text = "0";
      GameplayManager.Instance.PlayerDead();
   }

   public int GetPlayerHealth()
   {
      var curhealth = health.GetCurHealth();
      return curhealth;
   }

   public int GetPlayerMaxHealth()
   {
      var maxHealth = health.GetMaxHealth();
      return maxHealth;
   }

   public Transform GetPlayerBodyTransform()
   {
      return health.gameObject.transform;
   }
}
