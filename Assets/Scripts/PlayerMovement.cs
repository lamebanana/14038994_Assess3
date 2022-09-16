using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Animator animatorController;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake(){

        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            animatorController.SetTrigger("GoLeft");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            animatorController.SetTrigger("GoRight");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            animatorController.SetTrigger("GoUp");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            animatorController.SetTrigger("GoDown");
        }
    }

}
