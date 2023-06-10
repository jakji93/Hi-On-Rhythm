using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupController : MonoBehaviour
{
   [SerializeField] private TextMeshPro text;
   [SerializeField] AnimationCurve opacityCurve;
   [SerializeField] AnimationCurve scaleCurve;
   [SerializeField] AnimationCurve heightCurve;

   private float time = 0;
   private Vector3 preLocalScale;
   private Vector3 origin;

   private void Start()
   {
      preLocalScale = transform.localScale;
      origin = transform.localPosition;
   }

   public void SetDamageTaken(int damageTaken)
   {  
      text.text = "-" + damageTaken.ToString();
   }

   private void Update()
   {
      text.color = new Color(text.color.r, text.color.b, text.color.g, opacityCurve.Evaluate(time));
      transform.localScale = preLocalScale * scaleCurve.Evaluate(time);
      transform.localPosition = origin + new Vector3(0, heightCurve.Evaluate(time), 0);
      time += Time.deltaTime;
   }
}
