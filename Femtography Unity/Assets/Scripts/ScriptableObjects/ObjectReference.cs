using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "objectReference", menuName = "References/Object Reference", order = 1)]
public class ObjectReference : ScriptableObject
{
    public GameObject referencedGameObject;
}
