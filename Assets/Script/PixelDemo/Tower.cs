using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float m_CoolDown = 3;
    [SerializeField]
    private GameObject m_ShotPoint;
    private float m_LastShoot;
    [SerializeField]
    private GameObject m_BulletPrefab;
    
    [SerializeField]
    private PlayerController m_Owner;
    private Brush m_OwnerBrush;
    
    private Material m_Material;

    [SerializeField]
    private Ground m_Ground;
    [SerializeField]
    [InspectorName("占领半径")]
    private float m_Radius = 3;

    private Color32 m_SelfColor = s_DefaultColor;
    void Start()
    {
        m_Ground = GameObject.FindObjectOfType<Ground>();
        foreach (var r in GetComponents<Renderer>())
        {
            foreach (var m in r.materials)
            {
                m_Material = m;
            }
        }
    }

    public void SetOwner(PlayerController player)
    {
        m_Owner = player;
        m_OwnerBrush = player.GetComponent<Brush>();
        m_LastShoot = Time.time;
        m_Material.color = m_OwnerBrush.m_Color;
    }

    public void SetColor(Color32 color)
    {
        m_LastShoot = Time.time;
        m_Material.color = color;
    }

    private static Color32 s_DefaultColor = new Color32(0, 0, 0, 0);

    private bool CompareColor(Color32 color1, Color32 color2)
    {
        return !(color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a);
    }
    
    private void FixedUpdate()
    {
        var color = m_Ground.CheckOwner(transform.position, (int)m_Radius);
        if (CompareColor(color, m_SelfColor))
        {
            m_SelfColor = color;
            SetColor(m_SelfColor);
        }
        // if (m_Owner == null)
        //     return;

        if (!CompareColor(m_SelfColor, s_DefaultColor))
            return;
        
        if (Time.time - m_LastShoot >= m_CoolDown)
        {
            Shoot();
            m_LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        var bullet = GameObject.Instantiate(m_BulletPrefab);
        var bc = bullet.GetComponent<Bullet>();
        bc.m_Color = m_SelfColor;
        bc.m_Speed = Random.Range(20, 35);
        bullet.transform.position = m_ShotPoint.transform.position + Vector3.up * 2;
        bullet.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
