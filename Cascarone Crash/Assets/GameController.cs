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

    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] public Transform deadEnemiesParent;

    public bool lose;

    public Material[] mats;

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
        enemyCount = enemyParent.childCount;
        enemyCountText.text = "Enemies Remaining: " + enemyCount;
        if(enemyCount == 0)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
        if (lose)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void UpdateAmmo(int ammount)
    {
        ammoText.text = "Ammo: " + ammount;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
