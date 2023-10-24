using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

public class Build : MonoBehaviour
{
    public List<CubeGroundGrid> m_Grids;
 
    private Material m_Material;
    private Color m_Color;
    public Color Color => m_Color;
    private bool hasOwner;
    
    // 在网格上的坐标
    public Vector2 GridPos;
    void Awake()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            foreach (var m in r.materials)
            {
                m_Material = m;
            }
        }
        m_UnregisterCall = EventUtil.Instance.Register(this);
    }

    private Action m_UnregisterCall;
    private void OnDestroy()
    {
        m_UnregisterCall?.Invoke();
    }

    private void FixedUpdate()
    {
        if (m_Grids == null || hasOwner)
        {
            return;
        }

        foreach (var grid in m_Grids)
        {
            if (grid.Color != Color.red)
            {
                return;
            }
        }
        EventUtil.Instance.Send(new BuildActiveEvent
        {
            OwnerColor = Color.red
        });
    }
    
    [EventBus]
    public void OnSetOwner(BuildActiveEvent evt)
    {
        hasOwner = true;
        m_Color = evt.OwnerColor;
        m_Material.color = m_Color;
        EventUtil.Instance.Send(new GridDrawEvent
        {
            CenterPoint = GridPos,
            Radius = 5f
        });
        gameObject.SetActive(false);
        Destroy(this);
    }

    void Update()
    {
        
    }
}
