using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public AudioClip lighsOnClip;
    public AudioClip lighsOffClip;
    public AudioClip damageClip;
    public AudioClip shieldClip;

    AudioSource audioSource;

    public float speed;
    public float rotationSpeed;
    Animator anim;
    private bool lightsUp = false;
    private bool invulnerable = false;

    public GameObject[] lights;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        SetInvulnerable(2.0f);
    }

    void FixedUpdate()
    {
        Movement(this.speed, this.rotationSpeed);
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void Movement(float _speed, float _rotationSpeed)
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.up * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * _rotationSpeed));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 0, -1 * Time.deltaTime * _rotationSpeed));
        }
    }

    public void Hit(int damage)
    {
        if (!invulnerable && !GameManager.Instance.death)
        {
            audioSource.PlayOneShot(damageClip);
            GameManager.Instance.ScorePlus(0, -damage);
        }
    }
    public bool GetInvulnerable()
    {
        return invulnerable;
    }
    public void SetInvulnerable(float seconds, bool hit = true)
    {
        if (!invulnerable)
        {
            if(!hit)
                audioSource.PlayOneShot(shieldClip);
            anim.SetBool("Inmune", true);
            StartCoroutine(Invincible(seconds));
        }
            
    }

    IEnumerator Invincible(float seconds)
    {
        GameManager.Instance.DeactivateInmune();
        invulnerable = true;
        yield return new WaitForSeconds(seconds);
        //Activar animacion de parpadeo
        anim.SetBool("Inmune", false);
        invulnerable = false;
        GameManager.Instance.ActivateInmune();
    }

    public void MoreLight()
    {
        if (!lightsUp)
            StartCoroutine(LightsUp(5.0f));
    }

    IEnumerator LightsUp(float seconds)
    {
        audioSource.PlayOneShot(lighsOnClip);
        GameManager.Instance.DeactivateLights();
        lightsUp = true;
        lights[0].GetComponent<Light2D>().pointLightOuterRadius = 28;
        lights[1].GetComponent<Light2D>().pointLightOuterRadius = 12;
        yield return new WaitForSeconds(seconds);
        lights[0].GetComponent<Light2D>().pointLightOuterRadius = 14;
        lights[1].GetComponent<Light2D>().pointLightOuterRadius = 6;
        lightsUp = false;
        GameManager.Instance.ActivateLights();
        audioSource.PlayOneShot(lighsOffClip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Whirpool")
        {
            speed = Mathf.Clamp(speed-5, 0, 25);
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Whirpool")
        {
            speed = Mathf.Clamp(speed+5, 0, 25);
        }
    }

    public void LightsOff()
    {
        lights[0].GetComponent<Light2D>().pointLightOuterRadius = 0;
        lights[1].GetComponent<Light2D>().pointLightOuterRadius = 0;
    }
}
