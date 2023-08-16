using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TeleportAnchorNONXR : MonoBehaviour, IPointerClickHandler
{
    GameObject playerObject;

    [SerializeField] UnityEvent onClick;
    [SerializeField] GameObject teleportAnchor;
    [SerializeField] bool useTeleportAnchor;

    PointerEventData pointerEventData;

    float playerHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerHeight = GlobalVariableManager.Instance.PlayerHeight;

        int childCount = transform.childCount;

        if (teleportAnchor != null)
            return;
        for (int i = 0; i < childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "TeleportAnchor")
                teleportAnchor = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportPlayerToLocation()
    {
        Vector3 teleportLocation = this.transform.position;
        Quaternion teleportRotation = transform.rotation;

        if (useTeleportAnchor)
        {
            teleportLocation = teleportAnchor.transform.position;
            teleportRotation = teleportAnchor.transform.rotation;
        }

        playerObject = pointerEventData.pressEventCamera.gameObject;

        playerObject.transform.position = (new Vector3(
            teleportLocation.x,
            teleportLocation.y + playerHeight,
            teleportLocation.z
            ));
        playerObject.transform.rotation = teleportRotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pointerEventData = eventData;
        TeleportPlayerToLocation();
        onClick.Invoke();
    }
}
