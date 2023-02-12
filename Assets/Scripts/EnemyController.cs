using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int damage;
    public float maxSpeed;
    public float minSpeed = 1.0f;
    public float maxDistance;
    private bool moving = false;

    public float speed;
    Vector3 newPos;
    void Start()
    {
        SetMovement();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!moving)
        {
            SetMovement();
        }
        if (moving)
            Movement(maxSpeed);
    }
    void SetMovement()
    {
        float x = Random.Range(-65.0f, 65.0f);
        float y = Random.Range(-40.0f, 40.0f);
        speed = Random.Range(minSpeed, maxSpeed);
        newPos = new Vector3(x, y, 0);
        moving = true;
    }
    void Movement(float _speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        if(transform.position == newPos)
            moving = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            PlayerManager.Instance.Hit(this.damage);
            if(!PlayerManager.Instance.GetInvulnerable())
                PlayerManager.Instance.SetInvulnerable(1.0f);
        }
    }
}
