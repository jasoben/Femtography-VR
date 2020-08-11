using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public bool multiEventListener; // This allows us to respond to multiple events in the same way;
    public GameEvent Event;

    public List<GameEvent> Events;
    public UnityEvent Response;
    
    private void OnEnable()
    {
        if (!multiEventListener)
            Event.RegisterListener(this);

        if (multiEventListener)
        {
            foreach(GameEvent thisEvent in Events)
            {
                thisEvent.RegisterListener(this);
            }
        }
    }
    private void OnDisable()
    {
        if (!multiEventListener)
            Event.UnregisterListener(this);
        if (multiEventListener)
        {
            foreach(GameEvent thisEvent in Events)
            {
                thisEvent.UnregisterListener(this);
            }
        }
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
