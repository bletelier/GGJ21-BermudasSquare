using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirpoolScript : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public Transform following;
    public float rotationSpeed;
    void Start()
    {
        speed = maxSpeed;
        float xOffset = Random.Range(10.0f, 15.0f);
        float yOffset = Random.Range(10.0f, 15.0f);
        int xSign = Random.Range(0, 2) * 2 - 1;
        int ySign = Random.Range(0, 2) * 2 - 1;
        transform.position = new Vector3(xSign * (following.position.x + xOffset), ySign * (following.position.y + yOffset), 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * rotationSpeed));

        transform.position = Vector3.MoveTowards(transform.position, following.position, speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            if(!PlayerManager.Instance.GetInvulnerable())
            {
                GameManager.Instance.ScorePlus(0,-1000000);
            }
        }
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            GameManager.Instance.CreateEnemy();
        }
        if(collision.gameObject.tag == "Treasure")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PowerUpFreeze")
        {
            Destroy(collision.gameObject);
            StartCoroutine(Freeze(5.0f));
        }
    }

    IEnumerator Freeze(float seconds)
    {
        float _speed = speed;
        speed = 0.0f;
        yield return new WaitForSeconds(seconds);
        speed = _speed;
    }
}
