using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    Dictionary<int,UnitScriptableObject> spawnable = new Dictionary<int, UnitScriptableObject>();
    List<int> ids = new List<int>();
    // Start is called before the first frame update
    void Start()
    {

    }

    void AddSpawnable(UnitScriptableObject unit)
    {
        spawnable.Add(unit.id, unit);
        ids.Add(unit.id);
        ids.Sort();
        BroadcastMessage("DestroySpawnPanel");
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    
}
