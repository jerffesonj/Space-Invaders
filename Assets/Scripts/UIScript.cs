using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIScript : MonoBehaviour
{
    public GameObject gameOver;
    public TMP_Text pointsText;
    public TMP_Text gameOverPointsText;

    public AudioSource audioSource;
    public AudioClip gameOverSound;
    public AudioClip selectSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>().onPlayerDeath += PlayGameOver;
        GameOver.gameOverTrigger += PlayGameOver;
        EnemyManager.onGameOver += PlayGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        AttPoints();
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayGameOver();
        }
    }

    public void AttPoints()
    {
        pointsText.text = GameManager.instance.PlayerPoints.ToString();
    }
    public void PlayGameOver()
    {
        GameManager.instance.GetComponent<AudioSource>().Stop();
        audioSource.PlayOneShot(gameOverSound);
        gameOverPointsText.text = GameManager.instance.PlayerPoints.ToString() + " Points";
        gameOver.SetActive(true);    
        Time.timeScale = 0;
    }

    bool isRestarting = false;
    public void Restart()
    {
        StartCoroutine(RestartEnum());
    }

    IEnumerator RestartEnum()
    {
        if (isRestarting)
            yield break;
        isRestarting = true;
        SelectSound();
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void SelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }
}
