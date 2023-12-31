using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

public class CubeGroundGrid : MonoBehaviour
{
    private Material m_Material;
    private Color m_Color;
    public Color Color => m_Color;
    public int TeamIndex;
    
    private Vector3 m_DefaultPos;
    private Vector3 m_Offset = Vector3.zero;

    private GameObject m_MyBuild;
    public GameObject BuildPrefab;
    
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
        EventUtil.Instance.Register(this);
    }

    public void Build()
    {
        if (BuildPrefab != null)
        {
            if (m_MyBuild)
            {
                DestroyImmediate(m_MyBuild);
            }
            m_MyBuild = GameObject.Instantiate(BuildPrefab, transform, false);
        }
    }

    public void TeamChange(int index)
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            foreach (var m in r.materials)
            {
                m_Material = m;
            }
        }
        TeamIndex = index;
        switch (TeamIndex)
        {
            case 0:
                m_Color = Color.gray;
                break;
            case 1:
                m_Color = Color.red;
                break;
            case 2:
                m_Color = UnityEngine.Color.green;
                break;
        }
        m_Material.color = m_Color;
    }

    public void Init()
    {
        m_DefaultPos = transform.position;
    }

    private void FixedUpdate()
    {
        
    }
    
    public void Calculate(PlayerController player)
    {
        var position = player.transform.position;
        var position1 = transform.position;
        var dis = Mathf.Sqrt(Mathf.Pow(position1.x - position.x, 2) + Mathf.Pow(position1.z - position.z, 2));
        if (dis <= 0.8f)
        {
            m_Color = Color.red;
            m_Material.color = m_Color;
        }

        m_Offset.y = dis > 1f ? 0 : -0.1f;
        transform.position = m_DefaultPos + m_Offset;
    }

    [EventBus]
    public void OnDraw(GridDrawEvent evt)
    {
        if (Vector2.Distance(evt.CenterPoint, GridPos) <= evt.Radius)
        {
            m_Color = Color.red;
            m_Material.color = m_Color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
