using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_Speed = 30f;
    public float m_Gravity = -9.8f;
    private float m_DownSpeed;
    [InspectorName("爆炸范围")]
    public float m_Range = 5;
    public Color32 m_Color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Ground>(out var ground))
        {
            ground.Hit(transform.position, (int)m_Range, m_Color);
            gameObject.SetActive(false);
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        m_DownSpeed += m_Gravity * Time.fixedDeltaTime;
        transform.position += transform.rotation * new Vector3(m_Speed * Time.fixedDeltaTime, m_DownSpeed * Time.fixedDeltaTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
