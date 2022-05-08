using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

#if UNITY_EDITOR
public class EventListenerSorter : MonoBehaviour
{
    public void Sort()
    {
         var eventListeners = GetComponents<GameEventListener>().ToList();

         for (int p = 0; p <= eventListeners.Count - 2; p++)
         {
             for (int i = 0; i <= eventListeners.Count - 2; i++)
             {
                 GameEventListener eventListenerOne = eventListeners[i];
                 GameEventListener eventListenerTwo = eventListeners[i + 1];

                 string name1 = eventListenerOne.Event.name;
                 string name2 = eventListenerTwo.Event.name;

                 if (string.Compare(name1, name2) == 1)
                 {
                     UnityEditorInternal.ComponentUtility.MoveComponentUp(eventListenerTwo);
                     eventListeners = GetComponents<GameEventListener>().ToList();
                 }
             }
         }

         for (int i = 0; i < eventListeners.Count; i++)
         {
             UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
         }
    }
}
#endif
