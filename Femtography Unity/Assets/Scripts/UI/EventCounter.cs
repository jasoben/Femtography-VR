using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class EventCounter : MonoBehaviour
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
        CheckCount();
    }

    async Task CheckCount()
    {
        foreach (EventAtNumberOfClicks eventAtNumberOfClick in eventAtNumberOfClicks)
        {
            if (numberOfClick == eventAtNumberOfClick.clickCountToExecuteEvent)
            {
                await Task.Delay(eventAtNumberOfClick.pauseTime);
                eventAtNumberOfClick.eventToExecute.Invoke();
            }
        }
    }
}

[System.Serializable]
public class EventAtNumberOfClicks
{
    public int clickCountToExecuteEvent;
    public int pauseTime = 0;
    public UnityEvent eventToExecute;
}