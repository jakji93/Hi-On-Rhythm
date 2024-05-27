using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack5 : MonoBehaviour
{
   [SerializeField] private GameObject bomb;

   void Start()
   {
      if (bomb != null) Instantiate(bomb, transform.position, Quaternion.identity);
      Destroy(this.gameObject);
   }
}
