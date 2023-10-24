using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

public class FixedJoystick : Joystick
{
    private InputEvent m_InputEvent = new InputEvent();
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        base.HandleInput(magnitude, normalised, radius, cam);
        m_InputEvent.InputDir = normalised;
        EventUtil.Instance.Send(m_InputEvent);
    }
}