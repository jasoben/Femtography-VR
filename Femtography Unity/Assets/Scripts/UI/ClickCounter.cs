using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickCounter : MonoBehaviour
{
    [SerializeField]
    private EventAtNumberOfClicks[] eventAtNumberOfClicks;
    private int numberOfClick = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddClickCount()
    {
        numberOfClick++;
        CheckClickEvents();
    }

    void CheckClickEvents()
    {
        foreach (EventAtNumberOfClicks eventAtNumberOfClick in eventAtNumberOfClicks)
        {
            if (numberOfClick == eventAtNumberOfClick.clickCountToExecuteEvent)
                eventAtNumberOfClick.eventToExecute.Invoke();
        }
    }
}

[System.Serializable]
public class EventAtNumberOfClicks
{
    public int clickCountToExecuteEvent;
    public UnityEvent eventToExecute;
}