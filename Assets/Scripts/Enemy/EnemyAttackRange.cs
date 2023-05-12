using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackRange : MonoBehaviour
{
   [SerializeField] protected Transform attackPoint;

   public abstract bool IsInAttackRange();
}
