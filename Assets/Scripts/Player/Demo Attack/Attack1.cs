using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;



public class Attack1 : MonoBehaviour
{

    [SerializeField] private float atk1Dur = 2f;
    private float curDur;
   // Start is called before the first frame update
    void Start()
    {
        curDur = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (curDur <= atk1Dur )
        {
         curDur += Time.deltaTime;
        }
        else
        {
           Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.gameObject.tag == "Player") return;
        if(collision.gameObject.TryGetComponent<Health>(out Health health)) {
         health.TakeDamage(10 * ComboManager.Instance.GetMultiplier());
      }
    }
}
