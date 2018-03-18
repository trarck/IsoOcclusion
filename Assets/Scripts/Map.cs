using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField]
    GameObject m_TilePrefab;

    [SerializeField]
    int width = 10;

    [SerializeField]
    int height = 10;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                GameObject tile = GameObject.Instantiate(m_TilePrefab);
                tile.GetComponent<iso.Renderer.TileRenderer>().grid = new Vector2(i, j);
                tile.transform.SetParent(transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
