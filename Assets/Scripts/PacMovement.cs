using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMovement : MonoBehaviour
{ 

	public Transform pacman;
	private Animator anim;
	private AudioSource source;

	private string[] triggers = {"GoRight","GoDown", "GoLeft", "GoUp"};
	/*private Transform[] points = new Transform[4]; /*{{6,28, 0}, {12,28, 0},
									{12,24, 0}, {6,24, 0}};*/

	private Vector3[] patrol = {new Vector3(6,28, 0), new Vector3(12,28, 0), new Vector3(12,24, 0),
								new Vector3(6,24, 0)};

	private IEnumerator coroutine;

	/*[SerializeField]	
 	public float speed = 5f;*/

    void Start()
    {
		//GameObject prefab = pacman.GetComponent<AudioSource>();
		float r = Random.Range(0.5f, 1.1f);
		coroutine = move(r);
		source = pacman.GetComponent<AudioSource>();
		
		StartCoroutine(coroutine);
		
		
    }

	void Update(){
		
	}
     
     
    IEnumerator move(float waitTime)
	{
		while (true){
			
			for (int i = 0 ; i < 4; i++){

				anim = pacman.GetComponent<Animator>();
				anim.SetTrigger(triggers[i]);
				pacman.position = patrol[i];

				source.Play();
				
				yield return new WaitForSeconds(waitTime);
			}
		}
		
	}
    
}
