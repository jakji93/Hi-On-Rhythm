using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack4 : MonoBehaviour
{
   [SerializeField] private GameObject bomb;

   void Start()
   {  
      var mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePostion.z = 0;
      if (bomb != null) Instantiate(bomb, mousePostion, Quaternion.identity);
      Destroy(this.gameObject);
   }
}
