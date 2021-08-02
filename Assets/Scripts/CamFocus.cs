using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFocus : MonoBehaviour
{
    private GameObject target;

    private void Update()
    {
        if (!target)
        {
            target = GameObject.FindWithTag("Player");
        }
        else
            transform.position = target.transform.position;
    }
}
