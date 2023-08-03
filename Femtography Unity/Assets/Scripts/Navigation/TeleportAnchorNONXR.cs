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

    float playerHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerHeight = GlobalVariableManager.Instance.PlayerHeight;
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
            this.transform.position.y + playerHeight,
            this.transform.position.z
            ));
        playerObject.transform.rotation = transform.rotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pointerEventData = eventData;
        onClick.Invoke();
    }
}
