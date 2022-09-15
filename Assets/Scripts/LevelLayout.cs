using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class LevelLayout : MonoBehaviour
{

    public Sprite emptyTile, outsideCorner, outsideWall,
                        insideCorner, insideWall, pellet, junction;

    public GameObject powerPellet;

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

    private int[,] levelMapHFlip;
    private int[,] completeLevel = new int[30, 28];
    private int[,] halfLevelBottom = new int[15,28];
    private int[,] halfLevelUp = new int[15,28];

    private int[,] rotationMapHFlip;
    private int[,] completeRotation = new int[30, 28];
    private int[,] halfRotationBottom = new int[15,28];
    private int[,] halfRotationUp = new int[15,28];


// these rotations are calculated for corner sprites like |^ and centered walls like |
    private int [,] rotationMap = new int[15,14];

    private bool [,] rotationMap2 = new bool[15,14];

    private GameObject[,] tilesOrdered = new GameObject[30,28];


    void CreateRotationMap(){

        // level map [15, 14]


        for (int j = 0; j < 14; j ++){
            for (int i = 0; i < 15; i ++){

                
                rotationMap[i,j] = 0;
                rotationMap2[i,j] = false;

                // adjusting around empty/pellet tiles
                if (levelMap[i,j] == 0 || levelMap[i,j] == 5 || levelMap[i,j] == 6){

                    

                    if (i != 0){
                        if ((levelMap[i-1,j] == 2 || levelMap[i-1,j] == 4 || levelMap[i-1, j] == 7)){
                            //if(!rotationMap2[i-1,j])
                            rotationMap[i-1,j] = 90;
                            rotationMap2[i-1,j] = true;
                        } 

                    }

                    if (i != 14){
                        if ((levelMap[i+1,j] == 2 || levelMap[i+1,j] == 4 || levelMap[i+1, j] == 7)){
                                if(!rotationMap2[i+1,j])
                                    rotationMap[i+1,j] = 90;

                            rotationMap2[i+1,j] = true;
                        }
                    }

                    if (j != 0){

                        if ((levelMap[i,j-1] == 2 || levelMap[i,j-1] == 4 || levelMap[i, j-1] == 7)){
                            if(!rotationMap2[i,j-1])
                                rotationMap[i,j-1] = 0;

                            rotationMap2[i,j-1] = true;
                        }
                    } 
                    
                    if (j != 13){

                        if ((levelMap[i,j+1] == 2 || levelMap[i,j+1] == 4 || levelMap[i, j+1] == 7)){
                            if(!rotationMap2[i,j+1])
                            rotationMap[i,j+1] = 90;

                            rotationMap2[i,j+1] = true;
                        }
                    } 
                }

                //adjusting walls
                if (levelMap[i,j] == 2 || levelMap[i,j] == 4){

                    if (i != 0)
                    if (levelMap[i-1,j] == 0 || levelMap[i-1,j] == 5 || levelMap[i-1, j] == 6){
                        
                            rotationMap[i, j] = 90;
                            rotationMap2[i, j] = true;
                        
                            

                        
                    }
                }

                
                //if there is an internal corner on the edges, rotate it towards the extern of the maze
                if (levelMap[i,j] == 3){
                    if ( i == 0 && (levelMap[i+1,j] == 0 || levelMap[i+1,j] == 5 || levelMap[i+1,j] == 6)){
                        rotationMap[i,j] = 270;
                        rotationMap2[i,j] = true;
                    }

                    if ( i == 14 && (levelMap[i-1,j] == 0 || levelMap[i-1,j] == 5 || levelMap[i-1,j] == 6)){
                        rotationMap[i,j] = 90;
                        rotationMap2[i,j] = true;
                    }

                    if ( j == 13 && (levelMap[i,j-1] == 0 || levelMap[i,j-1] == 5 || levelMap[i,j-1] == 6)){
                        rotationMap[i,j] = 0;
                        rotationMap2[i,j] = true;
                    }
                }

                // adjusting corners
                if (levelMap[i,j] == 1 || levelMap[i,j] == 3){

                    if(i < 14 && j < 13){
                        if ((levelMap[i,j + 1] == 2 || levelMap[i,j + 1] == 4) && (levelMap[i+1,j] == 2 || levelMap[i+1,j] == 4)){


                            rotationMap[i,j] = 90;
                            rotationMap2[i,j] = true;

                        }

                        if ((levelMap[i,j + 1] == 1 || levelMap[i,j + 1] == 3) && (levelMap[i+1,j] == 2 || levelMap[i+1,j] == 4)){

                            
                            rotationMap[i,j] = 90;
                            rotationMap2[i,j] = true;

                        }

                        if ((levelMap[i+1,j] == 1 || levelMap[i+1,j] == 3) && (levelMap[i,j+1] == 2 || levelMap[i,j+1] == 4)){

                            //if(rotationMap2[i+1, j] || rotationMap2[i, j+1]){
                                rotationMap[i,j] = 90;
                                rotationMap2[i,j] = true;
                            //}
                        }
                      
                    }
                    
                    if(i > 0 && j > 0){
                        if ((levelMap[i,j - 1] == 2 || levelMap[i,j - 1] == 4) && (levelMap[i-1,j] == 2 || levelMap[i-1,j] == 4)){

                            //if(rotationMap2[i,j-1] || rotationMap2[i-1, j]){
                                rotationMap[i,j] = 270;
                                rotationMap2[i,j] = true;
                            //}

                        }

                        if ((levelMap[i,j - 1] == 1 || levelMap[i,j - 1] == 3) && (levelMap[i-1,j] == 2 || levelMap[i-1,j] == 4)){

                             //if(rotationMap2[i,j-1] || rotationMap2[i-1, j]){
                                rotationMap[i,j] = 270;
                                rotationMap2[i,j] = true;
                             //}
                        }

                        if ((levelMap[i-1,j] == 1 || levelMap[i-1,j] == 3) && (levelMap[i,j-1] == 2 || levelMap[i,j-1] == 4)){

                             //if(rotationMap2[i,j-1]){

                                    rotationMap[i,j] = 270;
                                    rotationMap2[i,j] = true;
                             //}
                        }
                    }

                

                    if (i < 14 && j > 0){
                        if ((levelMap[i+1,j] == 2 || levelMap[i+1,j] == 4) && (levelMap[i,j-1] == 2 || levelMap[i,j-1] == 4)){

                            if(rotationMap2[i+1,j] || rotationMap2[i, j-1]){
                                rotationMap[i,j] = 180;
                                rotationMap2[i,j] = true;
                            }
                            
                        }

                        if ((levelMap[i+1,j] == 1 || levelMap[i+1,j] == 3) && (levelMap[i,j-1] == 2 || levelMap[i,j-1] == 4)){

                            if(rotationMap2[i+1, j] || rotationMap2[i, j-1]){
                                rotationMap[i,j] = 180;
                                rotationMap2[i,j] = true;
                            }
                        }

                        if ((levelMap[i+1,j] == 2 || levelMap[i+1,j] == 4) && (levelMap[i,j-1] == 1 || levelMap[i,j-1] == 3)){

                            if(rotationMap2[i+1, j] || rotationMap2[i, j-1]){
                                rotationMap[i,j] = 180;
                                rotationMap2[i,j] = true;
                            }
                        }

                        
                    }

                    //adjusting pieces that are surrounded by tiles that arent empty or pellets

                    if(i < 13 && i > 0 && j < 13 && j > 0){
                        if (checkAround(i,j)){

                            
                            if (levelMap[i,j-1] == 4 && rotationMap[i, j-1] == 0){

                                if (levelMap[i-1, j]== 4 && rotationMap[i-1,j] == 0)
                                    rotationMap[i,j] = 0;

                                if (levelMap[i-1, j] == 3 && rotationMap[i-1, j] != 90){
                                    rotationMap[i,j] = 90;
                                }
                                rotationMap2[i,j] = true;
                            } else if (levelMap[i,j-1] == 4 && rotationMap[i, j-1] == 90){

                                if (levelMap[i-1, j]== 4 && rotationMap[i-1,j] == 90)
                                    rotationMap[i,j] = 180;

                                if (levelMap[i-1, j] == 3 && rotationMap[i-1, j] != 0){
                                    rotationMap[i,j] = 270;
                                }


                            }



                        }
                    }
  
                }


                //adjusting junctions
                if(levelMap[i,j] == 7){

                    if(i == 0) rotationMap[i,j] = 90; else
                        if(levelMap[i-1, j] == 0 || levelMap[i-1, j] == 5 || levelMap[i-1, j] == 6){
                            rotationMap[i,j] = 0;
                        }
                    if(j == 0) rotationMap[i,j] = 0; else
                        if(levelMap[i,j-1] == 0 || levelMap[i, j-1] == 5 || levelMap[i, j-1] == 6){
                            rotationMap[i,j] = 90;
                        }


                    if(j == 13) /*90 or 180*/  rotationMap[i,j] = 90; else
                        if(levelMap[i,j+1] == 0 || levelMap[i, j+1] == 5 || levelMap[i, j+1] == 6){
                            rotationMap[i,j] = 90;
                        }

                    if(i == 14) rotationMap[i,j] = 90; else
                        if(levelMap[i+1,j] == 0 || levelMap[i+1, j] == 5 || levelMap[i+1, j] == 6){
                            rotationMap[i,j] = 180;
                        }

                }
            }


        }

    

    }

    bool checkAround(int x, int y){
        bool result = false;
        if((levelMap[x-1,y] != 0) && (levelMap[x-1,y] != 5) && (levelMap[x-1,y] != 6)){
            if ((levelMap[x+1,y] != 0) && (levelMap[x+1,y] != 5) && (levelMap[x+1,y] != 6)){
                if ((levelMap[x,y+1] != 0) && (levelMap[x,y+1] != 5) && (levelMap[x,y+1] != 6)) {
                    if ((levelMap[x,y-1] != 0) && (levelMap[x,y-1] != 5) && (levelMap[x,y-1] != 6)){
                        result = true;
                    } 
                } 
            } 
        }
        
        
        return result;    
    }
    
    void SetPosition(){

        for (int i = 0; i < 29; i ++){
            for (int j = 0; j < 28; j++){

                // j è la y
                // i è la x
                if (j > 13 && i <15){ //quadrante in alto a sx
                   tilesOrdered[i,j].transform.eulerAngles = new Vector3(0,180,halfRotationBottom[i,j]);
                } else if (i < 15 && j <=14){ // quadrante in basso a sx
                    tilesOrdered[i,j].transform.eulerAngles = new Vector3(0,0,halfRotationBottom[i,j]);
                } else if (i >= 15){ // quadrante destro
                   tilesOrdered[i,j].transform.eulerAngles = new Vector3(180,180,halfRotationUp[i - 14,j]) ;
                    if (j <14){
                        tilesOrdered[i,j].transform.eulerAngles = new Vector3(180,0,halfRotationUp[i - 14,j]);
                    }
                }

                

                

            }
        }


    }


    void Start(){

        DestroyAllTiles();

        CreateRotationMap();
        FlipHorizontallyLevel();
        CreateBelowPart();
        FlipVerticallyLevel();
                
        for (int i = 0; i < 29; i++){
            for (int j = 0; j < 28; j++){
    
                if(i < 15)
                    
                    //hai semplicemente invertito i e j in drawtile
                    DrawTile(j,i, halfLevelBottom[i,j]);
                else 
                    DrawTile(j,i, halfLevelUp[i - 14,j]);
                
                     
            }


        }

        SetPosition();


        
    }
    
    
    

    void DestroyAllTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("GameController");

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
        
        
        for (int row = 0; row < rows; row++)
        {
            i = 0;

            for (int index = columns-1; index >= 0; index--)
            {
                levelMapHFlip[row, i] = levelMap[row,index];
                rotationMapHFlip[row,i] = rotationMap[row,index];
                
                i++;
            } 
        }
    }



    private void DrawTile(int x, int y, int val){
        GameObject g = new GameObject();
        g.gameObject.tag="GameController";
        g.transform.position = new Vector3(x, y);
        SpriteRenderer renderer = g.AddComponent<SpriteRenderer>();
        Animator animator = g.AddComponent<Animator>();
        
        
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
                var go = Instantiate(powerPellet, new Vector3(x,y), Quaternion.identity);
                go.gameObject.tag = "GameController";
                g = go;
            break;
            case 7:
                g.transform.position = new Vector3(x,y);
                renderer.sprite = junction;
            break;
        }

        tilesOrdered[y,x] = g;

    }
}