using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Team
{
    Red,
    Blue,
    Neutral
}
public class GameManager : MonoBehaviour
{
    [Header("Game flow")]
    [Tooltip("The game goes on indefinitely")]
    public bool infinite = true;
    public int turnsPerMatch;
    public float doubleDamagePhase;
    public float tripleDamagePhase;
    public GameObject[] maps;
    // Testing
    public bool specifiedMap = false;
    public int mapIndex;

    [HideInInspector]
    public float globalDamageMultiplier = 1f;

    public float rateIncrease = 2f;
    public LayerMask whatIsGround;
    [SerializeField] Bounds mapZone;

    // Scores
    public int redScore { get; private set; }
    public int blueScore { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI redScoreText;
    public TextMeshProUGUI blueScoreText;
    public TextMeshProUGUI damageMultiplierText;
    [SerializeField] TextMeshProUGUI ammoCounterRed;
    [SerializeField] TextMeshProUGUI ammoCounterBlue;

    [Header("Scene Loader")]
    [SerializeField] int menuSceneIndex;

    public static GameManager instance;
    GunDisplay gunNameRed;
    GunDisplay gunNameBlue;

    public float time_elapsed { get; private set; }
    int times = 0;

    int turns;
    void Awake()
    {
        // Consistent frame rate
        Application.targetFrameRate = 90;
        // Loading previous results
        turns = PlayerPrefs.GetInt("Turns", 0);
        blueScore = PlayerPrefs.GetInt("BlueScore", 0);
        redScore = PlayerPrefs.GetInt("RedScore", 0);

        instance = this;
        redScoreText.SetText(redScore.ToString());
        blueScoreText.SetText(blueScore.ToString());

        turns++;
        if (!infinite && turns > turnsPerMatch)
        {
            PlayerPrefs.SetInt("Turns", 0);
            PlayerPrefs.SetInt("BlueScore", 0);
            PlayerPrefs.SetInt("RedScore", 0);
        }

        Invoke(nameof(StartDoubleDamagePhase), doubleDamagePhase);
        Invoke(nameof(StartTripleDamagePhase), tripleDamagePhase);

        // Assign correct display for each player
        GunDisplay[] gunNames = FindObjectsOfType<GunDisplay>();
        foreach (GunDisplay display in gunNames)
        {
            if (display.team == Team.Red) gunNameRed = display;
            else gunNameBlue = display;

        }

        if (specifiedMap)
        {
            maps[mapIndex].SetActive(true);
        }
        else maps[Random.Range(0, maps.Length)].SetActive(true);
        time_elapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time_elapsed += Time.deltaTime;
        if (Input.GetButtonDown("Menu"))
        {
            times++;
        }
        if (times >= 2)
        {
            SceneManager.LoadScene(menuSceneIndex);
        }
    }

    public void CountRedScore(int redScoreNumber)
    {
        redScore += redScoreNumber;
        redScoreText.SetText(redScore.ToString());
    }

    public void CountBlueScore(int blueScoreNumber)
    {
        blueScore += blueScoreNumber;
        blueScoreText.SetText(blueScore.ToString());
    }

    public void ReloadScene()
    {
        PlayerPrefs.SetInt("Turns", turns);
        PlayerPrefs.SetInt("BlueScore", blueScore);
        PlayerPrefs.SetInt("RedScore", redScore);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void StartDoubleDamagePhase()
    {
        globalDamageMultiplier = 2f;
        damageMultiplierText.SetText($"x{globalDamageMultiplier}");
        damageMultiplierText.GetComponent<Animator>().SetTrigger("Popup");
    }

    void StartTripleDamagePhase()
    {
        globalDamageMultiplier = 3f;
        damageMultiplierText.SetText($"x{globalDamageMultiplier}");
        damageMultiplierText.GetComponent<Animator>().SetTrigger("Popup");
    }

    public void ClearGunUI(Team team)
    {
        if (team == Team.Neutral) return;
        if (gunNameRed == null || gunNameBlue == null || ammoCounterBlue == null || ammoCounterRed == null) return;

        if (team == Team.Red)
        {
            gunNameRed.ClearText();
            ammoCounterRed.SetText("");
        }
        else
        {
            gunNameBlue.ClearText();
            ammoCounterBlue.SetText("");
        }
    }

    public void UpdateGunUI(int currentMag, GunStats stats, Team team)
    {
        if (team == Team.Neutral) return;
        if (gunNameRed == null || gunNameBlue == null || ammoCounterBlue == null || ammoCounterRed == null) return;

        if (team == Team.Red)
        {
            gunNameRed.UpdateText(stats.gunName);
            ammoCounterRed.SetText(currentMag.ToString() + "/" + stats.clipSize.ToString());
        }
        else
        {
            gunNameBlue.UpdateText(stats.gunName);
            ammoCounterBlue.SetText(currentMag.ToString() + "/" + stats.clipSize.ToString());
        }
    }

    /// <summary>
    /// Take exact 3D position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool ValidPosition(Vector3 pos)
    {
        return !Physics2D.OverlapCircle(pos, .5f, whatIsGround);
    }
    
    private void OnDrawGizmos()
    {
        Vector3 drawPos = transform.position + mapZone.center;
        Gizmos.DrawWireCube(drawPos, mapZone.size);
    }
}
