using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movable : MonoBehaviour
{
    Rigidbody2D body;
    List<Vector2> moveQueue = new();
    Vector2 target;
    GameObject selectionClone;
    Collider2D col;
    float stuckTime = 0;
    const float TIMETILLSTUCK = .5F;
    Vector2 stuckVector = Vector2.zero;
    Unit unit;
    // Start is called before the first frame update
    /* TODO FOR MOVE SCRIPT:
     * 
     * [x] Make a working move queue
     * [x] Make selected objects move
     * [x] Make selected objects not run into eachother while moving
     * [x] **Formations**
     * 
     * 
     * FOR LATER
     * [] Add Height Layers? Slopes?
     * [] Add Actual PathFinding
     * [] Add Attack Move
     * 
     * 
     */
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        target = body.position;
        unit = GetComponent<Unit>();
        selectionClone = new GameObject($"{gameObject.name} Ghost");
        SpriteRenderer sp = selectionClone.AddComponent<SpriteRenderer>();
        sp.sprite = GetComponent<SpriteRenderer>().sprite;
        sp.color = new Color(1, 1, 1, .25F);
        selectionClone.transform.localScale = transform.localScale;
        selectionClone.SetActive(false);
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void AddToMoveQueue(Vector2 pos)
    {
        moveQueue.Add(pos);
    }

    public void MoveTo(Vector2 pos)
    {
        moveQueue.Clear();
        moveQueue.Add(pos);
    }

    bool Move()
    {
        if (moveQueue.Count == 0)
        {
            if (Vector2.Distance(target, body.position) > .1)
            {
                body.velocity = (target - body.position) * unit.movespeed;
                return false;
            }
            else
            {
                body.position = target;
                body.velocity = Vector2.zero;
                return true;
            }

        }


        if (Vector2.Distance(moveQueue[0], body.position) < .05)
        {
            body.position = moveQueue[0];
            if (moveQueue.Count == 1)
                target = moveQueue[0]; 
            moveQueue.RemoveAt(0);
            
        }
        else
        {
            if (col.IsTouchingLayers(LayerMask.GetMask("Units")))
            {
                stuckTime += Time.deltaTime;
                if (stuckTime > TIMETILLSTUCK)
                {
                    stuckTime = 0;
                    stuckVector = Random.insideUnitCircle;
                }
            }
            else
            {
                stuckVector = Vector2.zero;
                stuckTime = 0;
            }
            Vector2 delta = moveQueue[0] - body.position;
            delta.Normalize();
            body.velocity = delta * unit.movespeed + stuckVector;
        }

        return false;
    }

    public void MoveSelection(Vector2 pos)
    {
        if(!selectionClone.activeSelf)
            selectionClone.SetActive(true);
        selectionClone.transform.position = pos;
    }

    public void DisableSelection()
    {
        selectionClone.SetActive(false);
    }
}
