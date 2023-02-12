using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlong : MonoBehaviour
{
    public bool movingRight = true;
    public Transform der;
    public Transform izq;
    private void Update()
    {
        if(movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, der.position, 6 * Time.deltaTime);
            if(Mathf.Abs(transform.position.x - der.position.x) <= 0.000001f)
            {
                movingRight = false;
                transform.eulerAngles = new Vector3(0,180,0);
            }
        }
        if (!movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, izq.position, 6 * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - izq.position.x) <= 0.000001f)
            {
                movingRight = true;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
