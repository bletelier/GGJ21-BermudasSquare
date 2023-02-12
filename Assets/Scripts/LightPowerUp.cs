using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            PlayerManager.Instance.MoreLight();
            Destroy(this.gameObject);
        }
    }
}
