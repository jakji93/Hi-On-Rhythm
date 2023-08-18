using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSyncer : MonoBehaviour
{
   [SerializeField] int band;
   [SerializeField] float startScale, scaleMultiplier;

   private void Update()
   {
      var audioData = AudioAnalyzer.Instance.GetBufferBandNormalizedData(band);
      audioData = Mathf.Clamp01(audioData);
      transform.localScale = new Vector3(transform.localScale.x, (audioData * scaleMultiplier) + startScale, transform.localScale.z);
   }
}
