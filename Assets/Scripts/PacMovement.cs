using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMovement : MonoBehaviour
{ 

	public Transform pacman;
	private Animator anim;
	private AudioSource source;

	private string[] triggers = {"GoRight","GoDown", "GoLeft", "GoUp"};

	private Vector3[] startPoints = {new Vector3(6,27, 0), new Vector3(12,27, 0), new Vector3(12,23, 0),
								new Vector3(6,23, 0)};

	private Vector3[] endPoints = {new Vector3(12,27, 0), new Vector3(12,23, 0), new Vector3(6,23, 0),
								new Vector3(6,27, 0)};

    void Start()
    {
		StartCoroutine(MoveToPosition());
    }


    
	IEnumerator MoveToPosition()
    {

		float desiredDuration = 2.0f ;

		while (true){
			for (int i = 0 ; i < 4; i ++){

				float elapsedTime = 0.0f;

				while(elapsedTime < desiredDuration){

					pacman.position = Vector3.Lerp(startPoints[i], endPoints[i], elapsedTime/desiredDuration);
					elapsedTime += Time.deltaTime;
					//anim.SetTrigger(triggers[i]);
					yield return new WaitForEndOfFrame();
				}

			}
		

		}

		
    }


	


    
}
