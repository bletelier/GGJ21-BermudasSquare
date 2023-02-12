using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLightPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetType() == typeof(BoxCollider2D))
        {
            GameManager.Instance.MoreLight();
            Destroy(this.gameObject);
        }
    }
}
