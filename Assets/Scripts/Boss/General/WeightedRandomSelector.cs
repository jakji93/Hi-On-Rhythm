using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Select a random child to execute")]
public class WeightedRandomSelector : Composite
{
   [BehaviorDesigner.Runtime.Tasks.Tooltip("Seed the random number generator to make things easier to debug")]
   public int seed = 0;
   [BehaviorDesigner.Runtime.Tasks.Tooltip("Do we want to use the seed?")]
   public bool useSeed = false;

   [SerializeField] private int[] weights;

   // The random child index execution order.
   private Stack<int> childrenExecutionOrder = new Stack<int>();
   // The task status of the last child ran.
   private TaskStatus executionStatus = TaskStatus.Inactive;

   public override void OnAwake()
   {
      // If specified, use the seed provided.
      if (useSeed) {
         Random.InitState(seed);
      }
   }

   public override void OnStart()
   {
      // Select 1 random child
      SelectRandomChild();
   }

   public override int CurrentChildIndex()
   {
      // Peek will return the index at the top of the stack.
      return childrenExecutionOrder.Peek();
   }

   public override bool CanExecute()
   {
      // Continue exectuion if no task has return success and indexes still exist on the stack.
      return childrenExecutionOrder.Count > 0 && executionStatus != TaskStatus.Success;
   }

   public override void OnChildExecuted(TaskStatus childStatus)
   {
      // Pop the top index from the stack and set the execution status.
      if (childrenExecutionOrder.Count > 0) {
         childrenExecutionOrder.Pop();
      }
      executionStatus = childStatus;
   }

   public override void OnConditionalAbort(int childIndex)
   {
      // Start from the beginning on an abort
      childrenExecutionOrder.Clear();
      executionStatus = TaskStatus.Inactive;
      SelectRandomChild();
   }

   public override void OnEnd()
   {
      // All of the children have run. Reset the variables back to their starting values.
      executionStatus = TaskStatus.Inactive;
      childrenExecutionOrder.Clear();
   }

   public override void OnReset()
   {
      // Reset the public properties back to their original values
      seed = 0;
      useSeed = false;
   }

   private void SelectRandomChild()
   {
      int totalWeight = 0;
      foreach (var weight in weights) {
         totalWeight += weight;
      }
      int j = Random.Range(0, totalWeight) + 1;
      int i = 0;
      for(i = 0; i < weights.Length; ++i) {
         if (weights[i] >= j) {
            break;
         } else {
            j -= weights[i];
         }
      }
      if(i < children.Count) {
         childrenExecutionOrder.Push(i);
      } else {
         childrenExecutionOrder.Push(0);
      }
   }
}
