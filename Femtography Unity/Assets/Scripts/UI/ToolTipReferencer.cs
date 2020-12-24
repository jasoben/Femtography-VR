using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipReferencer : MonoBehaviour
{
    public ObjectReference toolTipObject;
    // Start is called before the first frame update
    void Start()
    {
        toolTipObject.referencedGameObject = this.gameObject;
    }
}
