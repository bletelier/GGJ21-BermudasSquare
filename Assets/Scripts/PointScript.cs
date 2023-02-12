using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public int points;
    public int extraTime;
    public bool cofre = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            Destroy(this.gameObject);
            GameManager.Instance.ScorePlus(points, extraTime);
            if (cofre)
                GameManager.Instance.NewCofre();
        }
    }
}
