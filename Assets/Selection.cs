using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public RectTransform rect;

    Vector3 mousePos;
    Vector3 p2, p3;
    Vector3 p1 = new Vector3();
    Vector2 pivot = new Vector2();


    Dictionary<string, GameObject> selections = new Dictionary<string, GameObject>();

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            rect.GetComponentInParent<Image>().enabled = false;
            select(rect);
        }


        

        if (Input.GetMouseButtonDown(0))
        {
            rect.position = new Vector3(mousePos.x,mousePos.y);
            p1 = rect.position;
        }

        if (Input.GetMouseButton(0))
        {
                p2 = Camera.main.WorldToScreenPoint(mousePos) - Camera.main.WorldToScreenPoint(p1);
                if (p2.x < 0)
                {
                    pivot.x = 1;
                    p2.x *= -1;
                }
                else
                {
                    pivot.x = 0;
                }
                if (p2.y < 0)
                {
                    pivot.y = 1;
                    p2.y *= -1;
                }
                else
                {
                    pivot.y = 0;
                }

                rect.pivot = pivot;


                rect.GetComponentInParent<Image>().enabled = true;
                rect.sizeDelta = p2;
        }
    }

    private void select(RectTransform transform)
    {
        Camera main = Camera.main;
        Vector2 corner1 = new();
        corner1.x = pivot.x == 1 ? main.ScreenToWorldPoint(transform.anchoredPosition3D - (Vector3)transform.sizeDelta).x : main.ScreenToWorldPoint(transform.anchoredPosition3D).x;
        corner1.y = pivot.y == 1 ? main.ScreenToWorldPoint(transform.anchoredPosition3D - (Vector3)transform.sizeDelta).y : main.ScreenToWorldPoint(transform.anchoredPosition3D).y;

        Vector2 corner2 = (Vector2)main.ScreenToWorldPoint(main.WorldToScreenPoint(corner1) + (Vector3)transform.sizeDelta);

        GameObject[] selectables = GameObject.FindGameObjectsWithTag("Selectable");

        foreach (GameObject obj in selectables)
        {
            if ((corner1.x < obj.transform.position.x && obj.transform.position.x < corner2.x) && (corner1.y < obj.transform.position.y && obj.transform.position.y < corner2.y))
            {
                if (!selections.ContainsKey(obj.name))
                { selections.Add(obj.name, obj); }
            }
            else
            {
                selections.Remove(obj.name);
            }
        }
    }
    public Dictionary<string, GameObject> getSelected()
    {
        return selections; 
    }
}
