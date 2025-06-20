using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    public static GameController GC;

    [SerializeField] TextMeshProUGUI enemyCountText;
    [SerializeField] public Transform enemyParent;
    int enemyCount;

    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI medalsText;


    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] public Transform deadEnemiesParent;

    public bool lose;

    public Material[] mats;

    [SerializeField] bool debugTimescale;

    [Range(0, 1)]
    [SerializeField] float newTimescale;

    [SerializeField] public Player player;

    private void OnEnable()
    {
        if (GC == null)
            GC = this;
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = enemyParent.childCount;
        enemyCountText.text = "Enemies Remaining: " + enemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(debugTimescale)
            Time.timeScale = newTimescale;
        enemyCount = enemyParent.childCount;
        enemyCountText.text = "Enemies Remaining: " + enemyCount;
        if(enemyCount == 0)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void UpdateAmmo(int ammount)
    {
        ammoText.text = "Ammo: " + ammount;
    }

    public void UpdateGold(int ammount)
    {
        medalsText.text = "Medals: " + ammount;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        player.tr.enabled = false;
        foreach (Transform de in deadEnemiesParent)
            de.GetComponent<Enemy>().tr.enabled = false;
        Invoke("ActuallyRestart", 0.01f);
    }

    void ActuallyRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
