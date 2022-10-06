using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{

	public Transform pacman;
	private Animator anim;
	private AudioSource source;

    private int lastInput = 0;


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
		anim = pacman.GetComponent<Animator>();
		source = pacman.GetComponent<AudioSource>();

        //StartCoroutine(MoveToPosition());
		
    }


    void Update(){

        if (Input.GetKey(KeyCode.A) || lastInput == 1){
            anim.Play("pac_student_left");
            StopCoroutine(MoveToPosition());
            lastInput = 1;

            
            
        } 
            
        if (Input.GetKey(KeyCode.D) || lastInput == 2){
            anim.Play("pac_student_right");
            StopCoroutine(MoveToPosition());
            lastInput = 2;

            
            
        }     
        if (Input.GetKey(KeyCode.W) || lastInput == 3){
            anim.Play("pac_student_movement_up");
            StopCoroutine(MoveToPosition());
            lastInput = 3;

            
            
        } 
            
        if (Input.GetKey(KeyCode.S) || lastInput == 4){
            anim.Play("pac_student_movement_down");
            StopCoroutine(MoveToPosition());
            lastInput = 4;
            
        }



        StartCoroutine(MoveToPosition());


    }

    IEnumerator MoveToPosition(){

        Vector3 startPoint = pacman.position;
        Vector3 endPoint = pacman.position;
        Vector3 newPosition = pacman.position;

        float lerpTime = 1.0f;
        float currentLerpTime = 0;
        
        currentLerpTime += Time.deltaTime * 4.0f;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        

        anim.enabled = true;

        if(lastInput == 1){
            
            endPoint.x = endPoint.x - 1;
            endPoint.x = Mathf.Lerp(pacman.position.x, endPoint.x, perc);
            pacman.position = endPoint;
        } else


        if (lastInput == 2){
            
            endPoint.x = endPoint.x + 1;
            endPoint.x = Mathf.Lerp(pacman.position.x, endPoint.x, perc);
            pacman.position = endPoint;
        } else

        if(lastInput == 3){
            
            endPoint.y = endPoint.y +1;
            endPoint.y = Mathf.Lerp(pacman.position.y, endPoint.y, perc);
            pacman.position = endPoint;
        } else

        if (lastInput == 4){
            
            endPoint.y = endPoint.y - 1;
            endPoint.y = Mathf.Lerp(pacman.position.y, endPoint.y, perc);
            pacman.position = endPoint;
        }
        

        yield return new WaitForEndOfFrame();
        
    }
    


    
}