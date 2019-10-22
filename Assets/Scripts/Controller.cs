using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Entity target;

    protected virtual void Start() { target = GetComponent<Entity>(); }
}
