using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] public int _aimType = 1;

    public int aimType { get { return _aimType; } 
        set {
            _aimType = value;
            PlayerPrefs.SetInt("MouseAim", value);
            player.ChangeAim();
        } }


    [Header("Settings")]
    [SerializeField] GameObject settings;
    [SerializeField] public TMP_Dropdown aimDropdown;
    [SerializeField] public Toggle mute;

    [SerializeField] public Sprite[] characters;
    [SerializeField] GameObject characterSelect;
    int characterIndex;

    string[] movements = { "Man", "Bird", "Cat", "Dog", "Rat" };
    [SerializeField] Image previewCharacter;

    [SerializeField] GameObject gameUI;

    private void OnEnable()
    {
        if (GC == null)
        {
            GC = this;
            characterSelect.SetActive(true);
            gameUI.SetActive(false);
        }

        characterIndex = PlayerPrefs.GetInt("Character");
        previewCharacter.sprite = characters[characterIndex];

        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        aimType = PlayerPrefs.GetInt("MouseAim");
        aimDropdown.value = PlayerPrefs.GetInt("MouseAim");
        enemyCount = enemyParent.childCount;
        enemyCountText.text = "Enemies Remaining: " + enemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterSelect.activeSelf)
            return;
        if(debugTimescale)
            Time.timeScale = newTimescale;
        enemyCount = enemyParent.childCount;
        enemyCountText.text = "Enemies Remaining: " + enemyCount;
        if(enemyCount == 0)
        {
            Win();
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

    public void Win()
    {
        CloseSettings();
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void Lose()
    {
        CloseSettings();
        loseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void AimToggle()
    {
        aimType = aimDropdown.value;
        Debug.Log(aimType);
    }

    public void MuteToggle()
    {
        //does nothing lol
    }

    public void OpenSettings()
    {
        if (!settings.activeSelf)
        {
            settings.SetActive(true);
            Time.timeScale = 0;
        }
        else
            CloseSettings();
    }
    public void CloseSettings()
    {
        settings.SetActive(false);
        Time.timeScale = 1;
    }

    public void IncrementCharacterIndex()
    {
        characterIndex++;
        characterIndex = characterIndex % characters.Length;
        Debug.Log(characterIndex);

        previewCharacter.sprite = characters[characterIndex];
    }
    public void DecrementCharacterIndex()
    {
        characterIndex--;
        if (characterIndex < 0)
            characterIndex = characters.Length - 1;
        Debug.Log(characterIndex);
        previewCharacter.sprite = characters[characterIndex];
    }

    public void SelectBtn()
    {
        PlayerPrefs.SetInt("Character", characterIndex);
        enemyParent.gameObject.SetActive(true);
        characterSelect.SetActive(false);
        gameUI.SetActive(true);
        ChangeCharacter();
    }

    public void ChangeCharacter()
    {
        player.playerMat.material.mainTexture = characters[characterIndex].texture;
        player.anim.Play(movements[characterIndex] + "Movement");
    }
}
