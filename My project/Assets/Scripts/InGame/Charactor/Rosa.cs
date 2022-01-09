using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rosa : Enemy
{
    public override void Init()
    {
        beginPos = new Vector2Int(25, 13);
        speed = 0.3f;
        base.Init();
    }

}
