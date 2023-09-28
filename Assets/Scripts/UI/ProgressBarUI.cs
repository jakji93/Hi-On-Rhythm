using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
   [SerializeField] private GameObject hasProgressObject;
   [SerializeField] private Image barImage;
   [SerializeField] private bool isDepleting = false;
   [SerializeField] private bool hideAtStart = false;
   [SerializeField] private UnityEvent onChange;
   [SerializeField] private Color fullColor;
   [SerializeField] private Color emptyColor;
   [SerializeField] private float colorThreshold;

   private IHasProgress hasProgress;

   private void Start()
   {
      hasProgress = hasProgressObject.GetComponent<IHasProgress>();
      if(hasProgress == null) {
         Debug.LogError("Object missing IHasProgress");
      }

      hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
      barImage.fillAmount = isDepleting ? 1f : 0f;
      if (barImage.fillAmount >= colorThreshold) {
         barImage.color = fullColor;
      } else {
         barImage.color = emptyColor;
      }
      if(hideAtStart) {
         Hide();
      }
   }

   private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
   {
      barImage.fillAmount = e.currentValue / e.maxValue;
      if(hideAtStart && (e.currentValue == 0 || e.currentValue == 1)) {
         Hide();
      } else {
         Active();
      }
      if (barImage.fillAmount >= colorThreshold) {
         barImage.color = fullColor;
      }
      else {
         barImage.color = emptyColor;
      }
      onChange?.Invoke();
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }

   private void Active()
   {
      gameObject.SetActive(true);
   }
}
