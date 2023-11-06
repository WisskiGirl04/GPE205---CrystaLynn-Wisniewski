using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The Add function will eventually add a powerup
    public void Add(PowerUp powerupToAdd)
    {
        powerupToAdd.Apply(this);
    }
    // The Add function will eventually add a powerup
    public void Remove(PowerUp powerupToRemove)
    {
        // TODO: Create the Remove() Method
    }
}
