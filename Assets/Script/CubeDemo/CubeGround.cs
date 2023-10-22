using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGround : MonoBehaviour
{
    public int m_Width = 5;
    public int m_Height = 10;

    public float m_GridSize = 1;
    public GameObject prefab;

    private CubeGroundGrid[,] m_Grids;

    private PlayerController m_Player;
    
    void Start()
    {
        m_Player = GameObject.FindObjectOfType<PlayerController>();
        InitGround();
        InitBuild(10, 10, 12, 12);
    }

    void InitGround()
    {
        m_Grids = new CubeGroundGrid[m_Width, m_Height];
        for(int i = 0; i < m_Width; i++)
        {
            for (int j = 0; j < m_Height; j++)
            {
                var grid = Instantiate(prefab, transform, true);
                grid.transform.position = new Vector3(i * m_GridSize - m_Width * m_GridSize / 2, transform.position.y, j * m_GridSize - m_Height * m_GridSize / 2);
                grid.name = $"Grid_{i}_{j}";
                m_Grids[i, j] = grid.GetComponent<CubeGroundGrid>();
                m_Grids[i, j].GridPos = new Vector2(i, j);
                m_Grids[i, j].Init();
            }
        }
    }
    

    public GameObject buildPrefab;
    void InitBuild(int ix, int iy, int jx, int jy)
    {
        var go = Instantiate(buildPrefab, transform, true);
        var build = go.GetComponent<Build>();
        build.m_Grids = new List<CubeGroundGrid>();
        build.transform.position = (m_Grids[ix, iy].transform.position + m_Grids[jx, jy].transform.position) / 2;
        for (int i = ix - 1; i <= jx + 1; i++)
        {
            for (int j = iy - 1; j <= jy + 1; j++)
            {
                if (i > jx || i < ix || j > jy || j < iy)
                    build.m_Grids.Add(m_Grids[i, j]);
            }
        }

        build.GridPos = new Vector2((ix + jx) / 2, (jx + jy) / 2);
    }

    private void FixedUpdate()
    {
        var pos = m_Player.transform.position;
        var posX = Mathf.RoundToInt((pos.x + m_Width * m_GridSize / 2) / m_GridSize);
        var posY = Mathf.RoundToInt((pos.z + m_Height * m_GridSize / 2) / m_GridSize);
        
        // Debug.Log($"坐标{posX} {posY}");
        int imin = Mathf.Clamp(posX - 1, 0, m_Width - 1);
        int imax = Mathf.Clamp(posX + 1, 0, m_Width - 1);
        int jmin = Mathf.Clamp(posY - 1, 0, m_Height - 1);
        int jmax = Mathf.Clamp(posY + 1, 0, m_Height - 1);
        
        for (int i = imin; i <= imax; i++)
        {
            for (int j = jmin; j <= jmax; j++)
            {
                m_Grids[i, j].Calculate(m_Player);
            }
        }
    }

    void Update()
    {
        
    }
}
