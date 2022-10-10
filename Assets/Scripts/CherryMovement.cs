using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMovement : MonoBehaviour
{

    public GameObject cherry;
    public float spawnTime = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnCherry", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        //if ()
        
    }


    void SpawnCherry()
    {
        int x = Random.Range(1, 29);
        int y = Random.Range(1,29);

        int x_axis = Random.Range(0,2);
        int y_axis = Random.Range(0,2);
        

        print(x + "," + y + ": x_h " + x_axis +" y_h " + y_axis);
        var position = new Vector3(29 * x_axis, 29* y_axis, 0);
        //var prefab = cherry;
        GameObject go = Instantiate(cherry, position, Quaternion.identity) as GameObject ;
        
        StartCoroutine(move(go, x, y, x_axis, y_axis));

        
    }


    IEnumerator move(GameObject prefab, int x, int y, int x_axis, int y_axis){

        float desiredDuration = 8.0f;

		var startPoint = prefab.transform.position;
        var kx = 30;
        var ky = 30;
        if (x_axis == 1)
		    kx = -30;
        if (y_axis == 1)
            ky = -30;

        var endPoint = new Vector3((startPoint.x + kx), (startPoint.y+ky),0);

        print(startPoint + " " + endPoint);	
		float elapsedTime = 0.0f;

        while(elapsedTime < desiredDuration){

            prefab.transform.position = Vector3.Lerp(startPoint, endPoint, elapsedTime/desiredDuration);
            elapsedTime += Time.deltaTime;

            
            
            yield return new WaitForEndOfFrame();
        }

	
		
        DestroyImmediate(prefab, true);
		
    }

    
}
