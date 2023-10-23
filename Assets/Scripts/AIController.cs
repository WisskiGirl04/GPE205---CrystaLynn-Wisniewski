using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make Decisions
        MakeDecisions();
        // Run the parents update
        base.Update();  
    }

    public void MakeDecisions()
    {
        Debug.Log("Make Decisions");
    }

}
