using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioClip slowClip;
    public AudioClip deathClip;
    public AudioClip lighsOnClip;
    public AudioClip lighsOffClip;
    public AudioClip coinClip;
    AudioSource audioSource;


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI deathText;

    int timeSurvived=0;

    public bool death = false;

    public GameObject globalLight;
    private bool lightsUp = false;
    
    public GameObject monsters;


    public GameObject[] whirpools;

    public bool spawnedWhir1 = false;
    public bool spawnedWhir2 = false;
    public bool spawnedWhir3 = false;

    public GameObject[] enemies;
    private bool slowed = false;

    public int puntuation = 0;
    public int timer = 60;

    public GameObject deathScreen;
    public GameObject scoreScreen;

    public GameObject powerUpsSlow;
    public GameObject powerUpSlow;
    public GameObject powerUpsGlobalLight;
    public GameObject powerUpGlobalLight;
    public GameObject powerUpsLights;
    public GameObject powerUpLights;
    public GameObject powerUpsInmune;
    public GameObject powerUpInmune;

    private int maxScore;

    public GameObject treasures;
    public GameObject treasure;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxScore = PlayerPrefs.GetInt("maxScore", 0);
        StartCoroutine(GameTimer());
        maxScoreText.text = "MaxScore: " + maxScore.ToString();
        scoreText.text = "Score: " + puntuation.ToString();
    }
    
    public void MoreLight()
    {
        if (!lightsUp)
            StartCoroutine(LightsUp(5.0f));

    }

    IEnumerator LightsUp(float seconds)
    {
        audioSource.PlayOneShot(lighsOnClip);
        DeactivateGlobalLight();
        lightsUp = true;
        globalLight.GetComponent<Light2D>().intensity += 0.2f;
        yield return new WaitForSeconds(seconds);
        globalLight.GetComponent<Light2D>().intensity -= 0.2f;
        lightsUp = false;
        ActivateGlobalLight();
        audioSource.PlayOneShot(lighsOffClip);
    }

    public void SetDeath(int _timeSurvived)
    {
        if(!death)
        {
            death = true;
            PlayerManager.Instance.speed = 0;
            PlayerManager.Instance.rotationSpeed = 0;
            PlayerManager.Instance.LightsOff();
            audioSource.PlayOneShot(deathClip);
            scoreScreen.gameObject.SetActive(false);
            deathScreen.gameObject.SetActive(true);
            deathText.text = "Score: " + puntuation.ToString() + "\nTimeSurvived: " + _timeSurvived.ToString() + "s";
        }
    }

    public void SlowWhirpools()
    {
        if (!slowed)
            StartCoroutine(SlowWhirpoolsCor(3.0f));
    }

    IEnumerator SlowWhirpoolsCor(float sec)
    {
        DeactivateSlow();
        foreach(GameObject whirpool in whirpools)
        {
            for (int i = 0; i < whirpool.transform.childCount; ++i)
            {
                whirpool.transform.GetChild(i).GetComponent<WhirpoolScript>().speed = 0.0f;
                whirpool.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.cyan;
            }
        }
        yield return new WaitForSeconds(sec);
        foreach (GameObject whirpool in whirpools)
        {
            for (int i = 0; i < whirpool.transform.childCount; ++i)
            {
                whirpool.transform.GetChild(i).GetComponent<WhirpoolScript>().speed = whirpool.transform.GetChild(i).GetComponent<WhirpoolScript>().maxSpeed;
                whirpool.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        ActivateSlow();
    }

    public void SlowMonsters()
    {
        if (!slowed)
        {
            audioSource.PlayOneShot(slowClip);
            StartCoroutine(Slow(3.0f));
        }
    }

    IEnumerator Slow(float seconds)
    {
        DeactivateSlow();
        float maxSpeed = monsters.transform.GetChild(0).GetComponent<EnemyController>().maxSpeed;
        float minSpeed = monsters.transform.GetChild(0).GetComponent<EnemyController>().minSpeed;
        for (int i = 0; i < monsters.transform.childCount; ++i)
        {
            monsters.transform.GetChild(i).GetComponent<EnemyController>().speed = 0.0f;
            monsters.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < monsters.transform.childCount; ++i)
        {
            monsters.transform.GetChild(i).GetComponent<EnemyController>().speed = Random.Range(minSpeed, maxSpeed);
            monsters.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
        }
        ActivateSlow();
    }

    public void DeactivateSlow()
    {
        powerUpsSlow.SetActive(false);
    }
    public void ActivateSlow()
    {
        powerUpsSlow.SetActive(true);
        Instantiate(powerUpSlow, powerUpsSlow.transform);
    }

    public void DeactivateGlobalLight()
    {
        powerUpsGlobalLight.SetActive(false);
    }
    public void ActivateGlobalLight()
    {
        powerUpsGlobalLight.SetActive(true);
        Instantiate(powerUpGlobalLight, powerUpsGlobalLight.transform);
    }
    public void DeactivateLights()
    {
        powerUpsLights.SetActive(false);
    }

    public void ActivateLights()
    {
        powerUpsLights.SetActive(true);
        Instantiate(powerUpLights, powerUpsLights.transform);
    }
    public void DeactivateInmune()
    {
        powerUpsInmune.SetActive(false);
    }

    public void ActivateInmune()
    {
        powerUpsInmune.SetActive(true);
        Instantiate(powerUpInmune, powerUpsInmune.transform);
    }

    IEnumerator GameTimer()
    {
        while (timer > 0 && !death)
        {
            yield return new WaitForSeconds(1.0f);
            timeSurvived++;
            if (timeSurvived % 30 == 0)
            {
                for (int i = 0; i < monsters.transform.childCount; ++i)
                {
                    float maxSpeed = monsters.transform.GetChild(i).GetComponent<EnemyController>().maxSpeed;
                    float minSpeed = monsters.transform.GetChild(i).GetComponent<EnemyController>().minSpeed;
                    monsters.transform.GetChild(i).GetComponent<EnemyController>().maxSpeed = Mathf.Clamp(maxSpeed + 3, 0, 30);
                    monsters.transform.GetChild(i).GetComponent<EnemyController>().minSpeed = Mathf.Clamp(minSpeed + 3, 0, 20);
                }
            }
            timer--;
            if (timer <= 30 && !spawnedWhir1)
            {
                spawnedWhir1 = true;
                SpawnWhirpools(0);
            }
            if (timer <= 20 && !spawnedWhir2)
            {
                spawnedWhir2 = true;
                SpawnWhirpools(1);
            }
            if (timer <= 10 && !spawnedWhir3)
            {
                spawnedWhir3 = true;
                SpawnWhirpools(2);
            }
            timeText.text = "TimeLeft: " + timer.ToString();
        }
        if (!death)
        {
            audioSource.PlayOneShot(deathClip);
            PlayerManager.Instance.speed = 0;
            PlayerManager.Instance.rotationSpeed = 0;
            PlayerManager.Instance.LightsOff();
            scoreScreen.gameObject.SetActive(false);
            deathScreen.gameObject.SetActive(true);
            deathText.text = "Score: " + puntuation.ToString() + "\nTimeSurvived: " + timeSurvived.ToString() + "s";
        }
    }

    public void ScorePlus(int points, int extraTime)
    {
        puntuation += points;
        scoreText.text = "Score: " + puntuation.ToString();
        if(puntuation > maxScore)
        {
            PlayerPrefs.SetInt("maxScore", puntuation);
            maxScore = puntuation;
            maxScoreText.text = "MaxScore: " + maxScore.ToString();
            StartCoroutine(ColorText(Color.green, maxScoreText));
        }
        if(extraTime > 0 || points>0)
        {
            audioSource.PlayOneShot(coinClip);
            StartCoroutine(ColorText(Color.green, timeText));
            StartCoroutine(ColorText(Color.green, scoreText));
        }
        else
        {
            StartCoroutine(ColorText(Color.red, timeText));
        }
            
        timer += extraTime;
        if(timer <= 0)
        {
            int _timeSurvived = timeSurvived;
            SetDeath(_timeSurvived);
        }
        timeText.text = "TimeLeft: " + timer.ToString();
    }

    IEnumerator ColorText(Color col, TextMeshProUGUI text)
    {
        text.color = col;
        yield return new WaitForSeconds(0.5f);
        text.color = Color.white;
    }
    public void SetTimer(int _timer)
    {
        this.timer = _timer;
        timeText.text = "TimeLeft: " + timer.ToString();
    }

    public void NewCofre()
    {
        Instantiate(treasure, treasures.transform);
    }

    public void CreateEnemy()
    {
        int idEnemy = Random.Range(0, enemies.Length);
        Instantiate(enemies[idEnemy], monsters.transform);
    }


    public void SpawnWhirpools(int id)
    {
        whirpools[id].SetActive(true);
    }
}
