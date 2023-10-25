using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ZOne;

public class MapEditor : MonoBehaviour
{
    [SerializeField]
    public LevelData m_LevelData;
    public GameObject m_BasePrefab;

    public List<CubeGroundGrid> m_Grids;
    public List<GameObject> m_Builds;

    public void Rebuild()
    {
        foreach (var g in m_Grids)
        {
            DestroyImmediate(g.gameObject);
        }
        m_Grids.Clear();
        
        for (int i = 0; i < m_LevelData.RectSize.x; i++)
        {
            for (int j = 0; j < m_LevelData.RectSize.y; j++)
            {
                var go = Instantiate(m_BasePrefab, transform, true);
                var grid = go.GetComponent<CubeGroundGrid>();
                grid.transform.position = new Vector3(i * m_LevelData.CellSize - m_LevelData.RectSize.x * m_LevelData.CellSize / 2,
                    transform.position.y,
                    j * m_LevelData.CellSize - m_LevelData.RectSize.y * m_LevelData.CellSize / 2);
                grid.name = $"Grid_{i}_{j}";
                grid.GridPos = new Vector2(i, j);
                grid.Init();
                m_Grids.Add(grid);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
