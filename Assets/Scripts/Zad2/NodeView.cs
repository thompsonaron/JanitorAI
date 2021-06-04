using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeView : MonoBehaviour
{
    public GameObject tile;

    public Node node;

    [Range(0f, 0.5f)]
    public float borderSize = 0.15f;

    public void Init(Node node)
    {
        if (tile != null)
        {
            gameObject.name = "Node (" + node.x + ", " + node.y + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f - borderSize, 1f - borderSize);
            this.node = node;
        }
    }

    private void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer renderer = go.GetComponent<SpriteRenderer>();

            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }

    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }
}
