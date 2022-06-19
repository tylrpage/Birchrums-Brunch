using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObjectSet")]
public class ObjectSet : ScriptableObject
{
    public List<ObjectController> objects;
}
