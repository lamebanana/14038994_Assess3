using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{

	public Transform pacman;
	private Animator anim;
	private AudioSource[] source;
    private ParticleSystem dust;

    private int lastInput = 0;
    private int currInput = 0;
    private int[] currPos, endPos; 

    private bool isLerping = false;


    private int[,] levelMapHFlip;
    private int[,] halfLevelBottom;
    private int[,] halfLevelUp;
    private int[,] completeLevel;

    float lerpTime = 1.0f;
    

    float perc = 0;

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

    void Start()
    {
        
        int columns = levelMap.GetLength(1);
        int rows = levelMap.GetLength(0);

        completeLevel = new int[columns * 2, rows *2];

		anim = pacman.GetComponent<Animator>();
		source = pacman.GetComponents<AudioSource>();

        dust = pacman.GetComponent<ParticleSystem>();

        var em = dust.emission;
        em.enabled = false;
        
        

        currPos = new int[2];
        endPos = new int [2];
        int x = (int)pacman.position.x;
        int y = (int)pacman.position.y;
        print("start pos: " + x + " " + y);

        currPos[0] = x;
        currPos[1] = y;


        
        FlipHorizontallyLevel();
        CreateBelowPart();
        FlipVerticallyLevel();


        for (int i = 0; i < ((rows*2)-1); i++){
            for (int j = 0; j < columns*2; j++){
    
                if(i < rows){
                    completeLevel[j,i] =  halfLevelBottom[i,j];
                }     
                else {

                    
                    completeLevel[j,i] = halfLevelUp[i - (rows-1),j];
                }
                    
                
                     
            }


        }


        //StartCoroutine(MoveToPosition());
		
    }


    void Update(){


        

        var em = dust.emission;
        //em.enabled = true;
        
        int x = (int)pacman.position.x;
        int y = (int)pacman.position.y;

        currPos[0] = x;
        currPos[1] = y;

        //endPos[0] = x;
        //endPos[1] = y;

        //var em = dust.emission;

        if (Input.GetKey(KeyCode.A ) || lastInput == 1){
            anim.Play("pac_student_left");
            
            lastInput = 1;
            currInput = 1;


            endPos[0] = currPos[0]-1;
            endPos[1] = currPos[1];

            em.enabled = true;

            
            
        }  
            
        if (Input.GetKey(KeyCode.D) || lastInput == 2){
            anim.Play("pac_student_right");
            
            lastInput = 2;
            currInput = 2;

            endPos[0] = currPos[0]+1;
            endPos[1] = currPos[1];

            em.enabled = true;

            
  
        }     
        if (Input.GetKey(KeyCode.W) || lastInput == 3){
            anim.Play("pac_student_movement_up");
            lastInput = 3;
            currInput = 3;

            endPos[1] = currPos[1]+1;
            endPos[0] = currPos[0];

            em.enabled = true;

            
            
        } 
            
        if (Input.GetKey(KeyCode.S) || lastInput == 4){
            anim.Play("pac_student_movement_down");
            lastInput = 4;
            currInput = 4;

            endPos[1] = currPos[1]-1;
            endPos[0] = currPos[0];

            em.enabled = true;

            
        }

        
        

        if (completeLevel[endPos[0],endPos[1]] == 0 || completeLevel[endPos[0],endPos[1]] == 6 || completeLevel[endPos[0],endPos[1]] == 5)
            if (!isLerping){
                StartCoroutine(MoveToPosition());
                //em.enabled = true;
            }// else em.enabled = false;
                
            


    }

    IEnumerator MoveToPosition(){

        Vector3 startPoint = new Vector3(currPos[0], currPos[1], 0);
        Vector3 endPoint = new Vector3(endPos[0], endPos[1], 0);

        
        float currentLerpTime = 0;
        

        while(currentLerpTime < lerpTime){
            currentLerpTime += Time.deltaTime;
            perc = currentLerpTime / lerpTime;

          

            pacman.position = Vector3.Lerp(startPoint, endPoint, perc *3.0f);
            isLerping = true;
            

            if (completeLevel[endPos[0],endPos[1]] == 0){
                while(!source[0].isPlaying){
                source[0].Play();
                }
            } else {
                while(!source[1].isPlaying){
                source[1].Play();}
            }
            yield return null;
            
            
        }

        

        
        isLerping = false;
        
         
        
        
    }


    void CreateBelowPart(){

        int columns = levelMap.GetLength(1);
        int rows = levelMap.GetLength(0);

        halfLevelBottom = new int[rows,columns*2];
        

        for (int i = 0; i < rows; i++){
            for (int j = 0; j < columns*2; j++){
                if(j < columns){
                    halfLevelBottom[i,j] = levelMap[i,j];
                    
                } else{
                    halfLevelBottom[i,j] = levelMapHFlip[i, j-columns];
                    
                }
            }
        }
    }

    void FlipVerticallyLevel()
    {
        
        

        int columns = halfLevelBottom.GetLength(1);
        int rows = halfLevelBottom.GetLength(0);

        halfLevelUp = new int[rows,columns];
        

        int i = 0;
  
        //fliph
        for (int column = 0; column < columns; column++)
        {
            i = 0;

            //flip row per each column
            for (int index = rows-1; index >= 0; index--)
            {
                halfLevelUp[i, column] = halfLevelBottom[index,column];
                
                i++;
            } 
        }

        
    }


    void FlipHorizontallyLevel()
    {
        
        int columns = levelMap.GetLength(1);
        int rows = levelMap.GetLength(0);
        int i = 0;

        levelMapHFlip = new int[rows,columns];
        
        
        
        for (int row = 0; row < rows; row++)
        {
            i = 0;

            for (int index = columns-1; index >= 0; index--)
            {
                levelMapHFlip[row, i] = levelMap[row,index];
                
                
                i++;
            } 
        }
    }
    


    
}