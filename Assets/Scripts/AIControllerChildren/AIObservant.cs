using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObservant : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.gameObject.name = "AIObservant Controller";
    }

    // Update is called once per frame
    public override void Update()
    {
        MakeDecisions();
    }
}
