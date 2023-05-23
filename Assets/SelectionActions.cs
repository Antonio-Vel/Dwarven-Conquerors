using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionActions : MonoBehaviour
{
    public Dictionary<string, GameObject> selected;
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

        if (Input.GetKeyDown("p"))
        { print("hi"); }
    }
}
