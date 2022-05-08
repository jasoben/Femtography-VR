using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Should this listener listen for multiple events prior to firing?")]
    public bool multiEventListener; // The multiEventListener allows us to respond to multiple events in the same way;
    [Tooltip("True if we want the 'Check Whether to Activate' bool to be true, false if we want it to be false")]
    public bool activateBoolOnOrOff;
    // activateBoolOnOrOff says whe, activateBoolOnOrOffther we want checkWhetherToActivate (below) to be on or off
    public GlobalBool checkWhetherToActivate; // This allows us to check a bool value before we invoke the event
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
        if (checkWhetherToActivate == null)
            Response.Invoke();
        else if (checkWhetherToActivate != null)
        {
            if ((checkWhetherToActivate.boolValue && activateBoolOnOrOff) 
                || (!checkWhetherToActivate.boolValue && !activateBoolOnOrOff))
                Response.Invoke();
        }
    }
}
