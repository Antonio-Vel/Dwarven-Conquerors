using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionActions : MonoBehaviour
{
    public Dictionary<string, GameObject> selected;
    Vector3 mousepos;
    Vector3 origin;
    bool LoR = false;
    const float SPACING = .5F, VERT = .5F;
    List<float> distances = new();
    Dictionary<float, Movable> movables = new();
    // Update is called once per frame
    void Update()
    {
        selected = Selection.getSelected();

        if (selected.Count == 0)
        { return; }

        if (Input.GetKey("p") || Input.GetKeyUp("p"))
        {
            Move(selected);
        }

    }
    //Code Museum

    //mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //foreach (KeyValuePair<string, GameObject> pair in selections)
    //{
    //    if (pair.Value.TryGetComponent<Movable>(out Movable component))
    //    {
    //        print($"{pair.Key} is moving to {Camera.main.ScreenToWorldPoint(Input.mousePosition)}!");
    //        if (Input.GetKey("w"))
    //        {
    //            component.AddToMoveQueue(mousepos);
    //        }
    //        else
    //        {
    //            component.MoveTo(mousepos);
    //        }
    //    }
    //    int rand = Random.Range(0, 360);
    //    mousepos = new Vector2((float)(mousepos.x + .5 * Mathf.Cos(rand)), (float)(mousepos.y + .5 * Mathf.Sin(rand)));
    //}
    private Vector2[] MakeFormation(int count, int rows, float rotation, Vector2 origin, Vector2 normDirection)
    {
        int[] rowsizes = new int[rows];
        for (int i = 0; i < rows; i++)
        {
            rowsizes[i] = count / rows;
        }
        for (int i = 0; i < count % rows; i++)
        {
            rowsizes[rowsizes.Length - 1 - i]++;
        }
        Vector2[][] rowsPos = new Vector2[rowsizes.Length][];
        Vector2 displacement = new Vector2(Mathf.Sin(rotation), Mathf.Cos(rotation)) * SPACING;
        int num = 0;
        for (int i = 0; i < rows; i++)
        {
            rowsPos[i] = new Vector2[rowsizes[i]];
            for (int c = 0; c < rowsizes[i]; c++)
            {
                if (rowsizes[i] % 2 == 0)
                {
                    if (c <= 1)
                    {
                        rowsPos[i][c] = origin + displacement * .5F * (c % 2 == 0 ? 1 : -1);
                        continue;
                    }
                    rowsPos[i][c] = rowsPos[i][c - 2] + displacement * (c % 2 == 0 ? 1 : -1);
                }
                else
                {
                    if (c == 0)
                    {
                        rowsPos[i][c] = origin;
                        continue;
                    }
                    if (c == 1)
                    {
                        rowsPos[i][c] = rowsPos[i][c - 1] + displacement * (c % 2 == 0 ? 1 : -1);
                        continue;
                    }

                    rowsPos[i][c] = rowsPos[i][c - 2] + displacement * (c % 2 == 0 ? 1 : -1);
                }
                num++;
            }
            origin += normDirection * VERT;
        }

        int z = 0;
        Vector2[] formation = new Vector2[count];
        for (int r = 0; r < rowsizes.Length; r++)
        {
            for (int c = 0; c < rowsizes[r]; c++)
            {
                formation[z] = rowsPos[r][c];
                z++;
            }
        }

        return formation;
    }

    void Move(Dictionary<string, GameObject> selections)
    {


        if (Input.GetKeyDown("p"))
        {
            origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (KeyValuePair<string, GameObject> pair in selections)
            {
                if (pair.Value.TryGetComponent<Movable>(out Movable comp))
                {
                    movables.Add(Vector2.Distance((Vector2)origin, pair.Value.GetComponent<Rigidbody2D>().position), comp);
                    distances.Add(Vector2.Distance((Vector2)origin, pair.Value.GetComponent<Rigidbody2D>().position));
                }
            }
            distances.Sort();
        }
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Sort by movable & distance from mousepos
        //Find position of mouse relative to original position in order to find rotation of formation
        Vector3 delta;
        float rotation;
        //print(Vector3.Distance(mousepos, origin));
        if (Vector3.Distance(mousepos, origin) != 0)
        {
            delta = mousepos - origin;
        }
        else
        {
            movables.TryGetValue(distances[0], out Movable m);
            delta = mousepos - m.GetComponent<Transform>().position;
        }
        rotation = Mathf.Atan2(delta.y,delta.x) ;
        //Create a formation of positions with rotation in mind
        int rows = (int)(Vector3.Distance(origin, mousepos) / VERT) + 1;
        delta.Normalize();
        Vector2[] formation = MakeFormation(movables.Count, rows, rotation, origin, delta);
        //Send out formation details on mouseup
        for (int i = 0; i < distances.Count; i++)
        {
            movables.TryGetValue(distances[i], out Movable m);
            m.MoveSelection(formation[i]);
        }
        if (Input.GetKeyUp("p"))
        {
            for (int i = 0; i < distances.Count; i++)
            {
                movables.TryGetValue(distances[i], out Movable m);
                if (Input.GetKey("w"))
                {
                    m.AddToMoveQueue(formation[i]);
                }
                else
                {
                    m.MoveTo(formation[i]);
                }
                m.DisableSelection();
                //print($"Ball {i} to {formation[i]}");
            }
            
            movables.Clear();
            distances.Clear();
        }
    }

}
