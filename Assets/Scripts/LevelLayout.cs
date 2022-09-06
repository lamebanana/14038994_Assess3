using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class LevelLayout : MonoBehaviour
{

    public Sprite emptyTile, outsideCorner, outsideWall,
                        insideCorner, insideWall, pellet, powerPellet, junction;


    public string[] leveltype = {"Full", "Bottom quarter", "Half bottom", "Half upper"};
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

    private int[,] levelMapHFlip, levelMapVFlip;
    private int[,] completeLevel = new int[30, 28];
    private int[,] halfLevelBottom = new int[15,28];
    private int[,] halfLevelUp = new int[15,28];

    private int[,] rotationMapHFlip, rotationMapVFlip;
    private int[,] completeRotation = new int[30, 28];
    private int[,] halfRotationBottom = new int[15,28];
    private int[,] halfRotationUp = new int[15,28];


// these rotations are calculated for croner sprites like |^ and centered walls like |
    private int [,] rotationMap =
    {
        {90,0,0,0,0,0,0,0,0,0,0,0,0,0}, 
        {90,0,0,0,0,0,0,0,0,0,0,0,0,90}, 
        {90,0,90,0,0,0,0,90,0,0,0,0,0,90},
        {90,0,90,0,0,90,0,90,0,0,0,90,0,90}, 
        {90,0,180,0,0,-90,0,180,0,0,0,-90,0,180}, 
        {90,0,0,0,0,0,0,0,0,0,0,0,0,0}, 


        {90,0,90,0,0,0,0,90,0,0,90,0,0,0,}, 
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

    private int[,] rotation, level;

    void Awake(){


        level = levelMap;
        rotation = rotationMap;

        for (int i = 0; i < halfLevelBottom.GetLength(0); i++){
                for (int j = 0; j < halfLevelBottom.GetLength(1); j++){ 
                    DrawTile(i,j, halfLevelBottom[i,j]);  
                }
            }
    }
    // Update is called once per frame
    void Update()
    {

        if (leveltype.Equals("Full")){

            level = completeLevel;
            rotation = completeRotation;

        }else if (leveltype.Equals("Half bottom")){
            level = halfLevelBottom;
            rotation = halfRotationBottom;
        }else if (leveltype.Equals("Half upper")){
            level = halfLevelUp;
            rotation = halfRotationUp;
        }else {
            level = levelMap;
            rotation = rotationMap;
        }

        if (update){

            DestroyAllTiles();
            update = false;

            //int c = 0;
            for (int i = 0; i < 30; i++){
                for (int j = 0; j < 28; j++){

                    FlipHorizontallyLevel();
                    CreateBelowPart();

                    FlipVerticallyLevel();
                    
                    if(i < 15)
                        DrawTile(i,j, halfLevelBottom[i,j]);
                    else {
                        DrawTile(i,j, halfLevelUp[i - 15,j]);
                    }
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

        int n = 0;
        for (int i = 0; i < 30; i ++){
            for (int j = 0; j < 28; j++){

                // j è la y
                // i è la x

                if (j > 13 && i <15){ //quadrante in alto a sx
                    /*if (completeLevel[i,j] == 3 || completeLevel[i,j] == 1){
                        tiles[n].transform.eulerAngles = new Vector3(180,0, halfRotationBottom[i,j]);
                    } else */tiles[n].transform.eulerAngles = new Vector3(180,0,halfRotationBottom[i,j]);
                } else if (i < 15 && j <=14){ // quadrante in basso a sx
                    tiles[n].transform.eulerAngles = new Vector3(0,0,halfRotationBottom[i,j]);
                } else if (i >= 15){ // quadrante destro
                    tiles[n].transform.eulerAngles = new Vector3(180,180,halfRotationUp[i - 15,j]);
                    if (j <14){
                        tiles[n].transform.eulerAngles = new Vector3(0,180,halfRotationUp[i - 15,j]);
                    }
                } 
                    
                
                n++;

                //if (n > 410) break;
            }
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


    void CreateBelowPart(){

        for (int i = 0; i < 15; i++){
            for (int j = 0; j < 28; j++){
                if(j < 14){
                    halfLevelBottom[i,j] = levelMap[i,j];
                    halfRotationBottom[i,j] = rotationMap[i,j];
                } else{
                    halfLevelBottom[i,j] = levelMapHFlip[i, j-14];
                    halfRotationBottom[i,j] = rotationMapHFlip[i, j -14];
                }
            }
        }
    }

    

    void FlipVerticallyLevel()
    {
        halfLevelUp = new int[15,28];
        halfRotationUp = new int[15,28];

        int columns = halfLevelBottom.GetLength(1);
        int rows = halfLevelBottom.GetLength(0);

        int i = 0;
  
        //fliph
        for (int column = 0; column < columns; column++)
        {
            i = 0;

            //flip row per each column
            for (int index = rows-1; index >= 0; index--)
            {
                halfLevelUp[i, column] = halfLevelBottom[index,column];
                halfRotationUp[i,column] = halfRotationBottom[index,column];
                i++;
            } 
        }

        
    }

    void FlipHorizontallyLevel()
    {
        levelMapHFlip = new int[15,14];
        rotationMapHFlip = new int[15,14];
        int columns = levelMap.GetLength(1);
        int rows = levelMap.GetLength(0);
        int i = 0;
        
        //fliph
        for (int row = 0; row < rows; row++)
        {
            i = 0;

            //flip row per each column
            for (int index = columns-1; index >= 0; index--)
            {
                levelMapHFlip[row, i] = levelMap[row,index];
                rotationMapHFlip[row,i] = rotationMap[row,index];
                i++;
            } 
        }

        //levelMap = levelMapHFlip;
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