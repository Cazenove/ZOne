using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Brush : MonoBehaviour
{
    private Collider m_Collider;

    public int m_Radius = 1;
    public Color32 m_Color = Color.black;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (Physics.SphereCast(transform.position + Vector3.up, m_Radius, Vector3.down, out var info, 5f, LayerMask.GetMask("Ground")))
        {
            if (info.collider.TryGetComponent<Ground>(out var ground))
            {
                ground.Hit(info.point, m_Radius, m_Color);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
