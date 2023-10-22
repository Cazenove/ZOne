using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    private Action m_UnregisterCall;
    private void Awake()
    {
        m_UnregisterCall = EventUtil.Register(this);
    }

    private void OnDestroy()
    {
        m_UnregisterCall?.Invoke();
    }

    private Vector3 m_InputDir;
    [EventBus]
    public void OnInput(InputEvent evt)
    {
        m_InputDir = new Vector3(evt.InputDir.x, 0, evt.InputDir.y);
        m_InputDir *= evt.magnitude;
    }

    public void FixedUpdate()
    {
        // Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(m_InputDir * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}