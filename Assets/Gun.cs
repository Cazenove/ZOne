using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

public class Gun : MonoBehaviour
{
    private void Awake()
    {
        EventUtil.Instance.Register(this);
    }

    // [EventBus]
    // public void OnSetOwner()
    // {
    //     
    // }

    private void FixedUpdate()
    {
        
    }
}
