using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    private int width;
    private int height;

    public TextAsset textAsset;
    public string resourcePath = "Mapdata";

    public Color32 openColor = Color.white;
    public Color32 closedColor = Color.black;
    static Dictionary<Color32, NodeType> terrainLookupTable = new Dictionary<Color32, NodeType>();

    void Awake()
    {
        SetupLookupTable();
    }

    void SetupLookupTable()
    {
        terrainLookupTable.Add(openColor, NodeType.Open);
        terrainLookupTable.Add(closedColor, NodeType.Closed);
    }

    void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (textAsset == null)
        {
            textAsset = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }
    }
    
    public List<string> GetMapFromTextFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

        if (tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\r\n", "\n" };
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MAPDATA GetTextFromFile Error: invalid TextAsset");
        }
        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(textAsset);
    }

    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach (string line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    // create map from text lines
    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();
        lines = GetMapFromTextFile(textAsset);

        SetDimensions(lines);

        int[,] map = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }
        return map;
    }

    public static Color GetColorFromNodeType(NodeType nodeType)
    {
        if (terrainLookupTable.ContainsValue(nodeType))
        {
            Color colorKey = terrainLookupTable.FirstOrDefault(x => x.Value == nodeType).Key;
            return colorKey;
        }

        return Color.white;
    }
}
