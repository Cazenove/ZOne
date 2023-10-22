using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ZOne;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_MoveSpeed = 3;
    
    private NewControls m_Controls;

    private void Awake()
    {
        EventUtil.Register(this);
    }

    void Start()
    {
        m_Controls = new NewControls();
        m_Controls.Newactionmap.Move.performed += MovePerform;
        m_Controls.Enable();
        
    }

    private Vector2 m_InputDir = Vector2.zero;

    private void MovePerform(InputAction.CallbackContext obj)
    {
        SetMoveDir(obj.ReadValue<Vector2>().normalized);
    }

    [EventBus]

    public void OnInput(InputEvent evt)
    {
        SetMoveDir(evt.InputDir);
    }
    
    public void SetMoveDir(Vector2 dir)
    {
        m_InputDir = dir;
        if (dir != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(m_InputDir.x, 0, m_InputDir.y));
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(m_InputDir.x, 0, m_InputDir.y) * (m_MoveSpeed * Time.fixedDeltaTime);
    }
}
