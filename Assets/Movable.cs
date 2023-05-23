using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movable : MonoBehaviour
{
    GameObject obj;
    Stack<Vector2> moveQueue = new();
    // Start is called before the first frame update
    /* TODO FOR MOVE SCRIPT:
     * 
     * [] Make a working move queue
     * [] Make selected objects move
     * [] Make selected objects not run into eachother while moving
     * 
     * 
     * FOR LATER
     * [] Add Height Layers? Slopes?
     * [] Add Actual PathFinding
     * [] Add Attack Move
     * 
     * 
     */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool move()
    {
        if (moveQueue.Count == 0)
        {
            return true;
        }

        return false;
    }
}
