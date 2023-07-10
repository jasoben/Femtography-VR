using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TeleportAnchorNONXR : MonoBehaviour, IPointerClickHandler
{
    GameObject playerObject;

    [SerializeField] UnityEvent onClick;

    PointerEventData pointerEventData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportPlayerToLocation()
    {
        playerObject = pointerEventData.pressEventCamera.gameObject;

        playerObject.transform.position = (new Vector3(
            this.transform.position.x,
            playerObject.transform.position.y,
            this.transform.position.z
            ));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pointerEventData = eventData;
        onClick.Invoke();
    }
}
