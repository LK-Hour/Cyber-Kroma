using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    void Start()
    {
        gameObject.tag = "Virus"; // make sure virus has this tag
    }

    public void OnHit()
    {
        Destroy(gameObject);
    }
}
