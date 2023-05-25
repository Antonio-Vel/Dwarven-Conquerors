using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionActions : MonoBehaviour
{
    public Dictionary<string, GameObject> selected;
    Vector2 mousepos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        selected = Selection.getSelected();

        if (selected.Count == 0)
        { return; }

        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown("p"))
        {
            foreach (KeyValuePair<string, GameObject> pair in selected)
            {
                print("howdy");

                if (pair.Value.TryGetComponent<Movable>(out Movable component))
                {
                    print($"{pair.Key} is moving to {Camera.main.ScreenToWorldPoint(Input.mousePosition)}!");
                    if (Input.GetKey("w"))
                    {
                        component.AddToMoveQueue(mousepos);
                    }
                    else
                    {
                        component.MoveTo(mousepos);
                    }
                }
                int rand = Random.Range(0, 360);

                mousepos = new Vector2(mousepos.x + .5 * Mathf.Cos(rand), mousepos.y + .5 * Mathf.)
            }
        }
    }
}
