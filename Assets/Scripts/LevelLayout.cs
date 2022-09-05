using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class LevelLayout : MonoBehaviour
{

    public Sprite emptyTile, outsideCorner, outsideWall,
                        insideCorner, insideWall, pellet, powerPellet, junction;

    public bool update = false;
    public bool set_position = false;
    private int[,] levelMap = 
    { 
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4}, 
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5}, 
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4}, 
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3}, 
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4}, 
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4}, 
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3}, 
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0}, 
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0}, 
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0}, 
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}, 
    }; 

    private GameObject[,] tiles;

// these rotations are calculated for croner sprites like |^ and centered walls like |
    private int [,] rotationMap =
    {
        {90,0,0,0,0,0,0,0,0,0,0,0,0,0}, 
        {90,0,0,0,0,0,0,0,0,0,0,0,0,90}, 
        {90,0,90,0,0,0,0,90,0,0,0,0,0,90}, // ricontrolla questa riga
        {90,0,90,0,0,90,0,90,0,0,0,90,0,90}, 
        {90,0,180,0,0,-90,0,180,0,0,0,-90,0,180}, 
        {90,0,0,0,0,0,0,0,0,0,0,0,0,0}, 
        {90,0,90,0,0,0,0,0,90,0,90,0,0,0}, 
        {90,0,180,0,0,-90,0,90,90,0,180,0,0,0}, 
        {90,0,0,0,0,0,0,90,90,0,0,0,0,90}, 
        {180,0,0,0,0,0,0,90,180,0,0,0,0,90}, 
        {0,0,0,0,0,90,0,90,90,0,0,-90,0,180}, 
        {0,0,0,0,0,90,0,90,90,0,0,0,0,0}, 
        {0,0,0,0,0,90,0,90,90,0,90,0,0,0},
        {0,0,0,0,0,-90,0,180,-90,0,90,0,0,0}, 
        {0,0,0,0,0,0,0,0,0,0,90,0,0,0}, 
    };

    private int[] rotationMap2 = {
        90,0,0,0,0,0,0,0,0,0,0,0,0,0, 
        90,0,0,0,0,0,0,0,0,0,0,0,0,90, 
        90,0,90,0,0,0,0,90,0,0,0,0,0,90, 
        90,0,90,0,0,90,0,90,0,0,0,90,0,90, 
        90,0,180,0,0,-90,0,180,0,0,0,-90,0,180, 
        90,0,0,0,0,0,0,0,0,0,0,0,0,0, 
        90,0,90,0,0,0,0,90,0,0,90,0,0,0,
        90,0,180,0,0,-90,0,90,90,0,180,0,0,0, 
        90,0,0,0,0,0,0,90,90,0,0,0,0,90, 
        180,0,0,0,0,0,0,90,180,0,0,0,0,90, 
        0,0,0,0,0,90,0,90,90,0,0,-90,0,180, 
        0,0,0,0,0,90,0,90,90,0,0,0,0,0, 
        0,0,0,0,0,90,0,90,90,0,90,0,0,0,
        0,0,0,0,0,-90,0,180,-90,0,90,0,0,0, 
        0,0,0,0,0,0,0,0,0,0,90,0,0,0 
    };

    void Awake(){
        for (int i = 0; i < levelMap.GetLength(0); i++){
                for (int j = 0; j < levelMap.GetLength(1); j++){ 
                    DrawTile(i,j, levelMap[i,j]);  
                }
            }
    }
    // Update is called once per frame
    void Update()
    {
        if (update){

            DestroyAllTiles();
            update = false;

            for (int i = 0; i < levelMap.GetLength(0); i++){
                for (int j = 0; j < levelMap.GetLength(1); j++){
                    DrawTile(i,j, levelMap[i,j]);
                }
            }
        }

        if (set_position){
            set_position = false;
            SetPosition();
        }

    }

    void SetPosition(){

        //rotationMap2[91] = 90;
        //rotationMap2[92] = 0;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tiles");

        for(int n = 0; n < tiles.Length; n++)
        {
            tiles[n].transform.eulerAngles = new Vector3(0,0,rotationMap2[n]);
        }
            
    }

    void DestroyAllTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tiles");

        for(int i = 0; i < tiles.Length; i++)
        {
            DestroyImmediate(tiles[i]);
        }
    }

    

    private void DrawTile(int x, int y, int val){
        GameObject g = new GameObject();
        g.gameObject.tag="Tiles";
        g.transform.position = new Vector3(x, y);
        SpriteRenderer renderer = g.AddComponent<SpriteRenderer>();
        
        switch (val)
        {
            case 0:
                renderer.sprite = emptyTile;
            break;
            case 1:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = outsideCorner;
            break;
            case 2:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = outsideWall;
            break;
            case 3:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = insideCorner;
            break;
            case 4:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = insideWall;
            break;
            case 5:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = pellet;
            break;
            case 6:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = powerPellet;
                
            break;
            case 7:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = junction;
            break;
        }
    }
}
