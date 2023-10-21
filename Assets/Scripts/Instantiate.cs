using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepointTransform;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newBullet = Instantiate(bulletPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject;
        Debug.Log(newBullet.name);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
