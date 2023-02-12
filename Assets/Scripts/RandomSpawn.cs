using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float _x = Random.Range(-100, 100);
        float _y = Random.Range(-100, 100);
        transform.position = new Vector3(_x, _y, 0);
    }
}
