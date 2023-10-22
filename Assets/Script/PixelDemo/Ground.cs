using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Ground : MonoBehaviour
{
    private Texture2D m_RenderTexture;

    [InspectorName("判定密度")]
    public int m_Cell = 1024;
    private Color32[] m_LogicPoints;
    private Material m_Material;

    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        m_LogicPoints = new Color32[m_Cell * m_Cell];
        m_RenderTexture = new Texture2D(m_Cell, m_Cell);
        foreach (var r in GetComponents<Renderer>())
        {
            foreach (var m in r.materials)
            {
                m_Material = m;
                m.mainTexture = m_RenderTexture;
            }
        }
    }

    /// <summary>
    /// 计算点在模型上的像素坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    private Vector2Int GetPixelByWorldPosition(Vector3 worldPos)
    {
        Transform transform1;
        var localPos = (transform1 = transform).InverseTransformPoint(worldPos);
        var localScale = transform1.localScale;
        Vector2 uv = new Vector2((localPos.x + localScale.x / 2) / localScale.x,
            (localPos.z + localScale.z / 2) / localScale.z) * m_Cell;
        return new Vector2Int(m_Cell - Mathf.RoundToInt(uv.x), m_Cell - Mathf.RoundToInt(uv.y));
    }

    /// <summary>
    /// 检测是否被某种颜色包围
    /// </summary>
    /// <returns></returns>
    public Color32 CheckOwner(Vector3 pos, int radius)
    {
        Vector2Int pixel = GetPixelByWorldPosition(pos);
        Dictionary<Color32, int> colorCounts = new Dictionary<Color32, int>();
        int totalPixel = 0;
        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; j <= radius; j++)
            {
                Vector2 v = new Vector2(pixel.x + (i - radius / 2f), pixel.y + (j - radius / 2f));
                if (v.x >= 0 && v.x <= m_Cell && v.y >= 0 && v.y <= m_Cell && (v - pixel).magnitude <= radius)
                {
                    totalPixel++;
                    if (!colorCounts.ContainsKey(m_LogicPoints[Mathf.RoundToInt(v.x) + Mathf.RoundToInt(v.y) * m_Cell]))
                    {
                        colorCounts[m_LogicPoints[Mathf.RoundToInt(v.x) + Mathf.RoundToInt(v.y) * m_Cell]] = 0;
                    }
                    colorCounts[m_LogicPoints[Mathf.RoundToInt(v.x) + Mathf.RoundToInt(v.y) * m_Cell]]++;
                }
            }
        }

        foreach (var kv in colorCounts)
        {
            if ((float)kv.Value / totalPixel >= 0.8f)
            {
                return kv.Key;
            }
        }

        return new Color32(0, 0, 0, 0);
    }

    public void Hit(Vector3 pos, int radius, Color32 color)
    {
        var pixel = GetPixelByWorldPosition(pos);

        m_LogicPoints[Mathf.RoundToInt(pixel.x) + Mathf.RoundToInt(pixel.y) * m_Cell] = color;
        for (int i = 0; i <= radius; i++)
        {
            for (int j = 0; j <= radius; j++)
            {
                Vector2 v = new Vector2(pixel.x + (i - radius / 2f), pixel.y + (j - radius / 2f));
                if (v.x >= 0 && v.x <= m_Cell && v.y >= 0 && v.y <= m_Cell && Vector2.Distance(v, pixel) <= radius)
                {
                    m_LogicPoints[Mathf.RoundToInt(v.x) + Mathf.RoundToInt(v.y) * m_Cell] = color;
                }
            }
        }
        m_RenderTexture.SetPixels32(m_LogicPoints);
        m_RenderTexture.Apply();
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
