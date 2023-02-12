using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public GameObject[] clouds;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        int ind = Random.Range(0, clouds.Length);
        float yOffset = Random.Range(-1.0f, 1.0f);
        Instantiate(clouds[ind], new Vector3(-transform.position.x + 5.0f, transform.position.y + yOffset, transform.position.z), transform.rotation);
    }
}
