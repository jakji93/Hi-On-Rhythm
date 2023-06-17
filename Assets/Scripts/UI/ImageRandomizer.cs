using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRandomizer : MonoBehaviour
{
   [SerializeField] private Image backgroundImage;
   [SerializeField] private Sprite[] sprites;

   private void Awake()
   {
      int index = Random.Range(0, sprites.Length);
      Debug.Log(index);
      backgroundImage.sprite = sprites[index];
   }
}
