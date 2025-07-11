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
    int _enemyCount;
    int enemyCount
    {
        get { return _enemyCount; }
        set
        {
            if (_enemyCount != value)
            {
                ChangeTarget();
            }
            _enemyCount = value;
        }
    }

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

    public string[] movements = { "Bird", "Cat", "Dog", "Rat", "Hedgehog", "Armadillo", "Hare" };
    [SerializeField] Image previewCharacter;

    [SerializeField] GameObject gameUI;

    [SerializeField] Texture2D[] textures;

    [SerializeField] public Image[] ammoEggs;

    public int eggIndex = 0;

    [SerializeField] compass theCompass;

    [SerializeField] GameObject creditsPage;
    
    [SerializeField] GameObject armoredIcon;

    public ParticleSystemPool confettiPop;

    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;

    bool _begin = false;
    public bool begin { get { return _begin; } set { _begin = value; if (value) player.retical.SetActive(true); } }

    [SerializeField] GameObject howToPlayPage;

    [SerializeField] GameObject resourcesView;

    [SerializeField] GameObject closeGame;
    RuntimePlatform platform;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");
            if(PlayerPrefs.GetFloat("Volume") == 0)
            {
                mute.isOn = true;
            }
            else
            {
                mute.isOn = false;
            }
        }
        if (GC == null)
        {
            GC = this;
            characterSelect.SetActive(true);
            gameUI.SetActive(false);
        }

        characterIndex = PlayerPrefs.GetInt("Character");
        previewCharacter.sprite = characters[characterIndex];

        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        aimType = PlayerPrefs.GetInt("MouseAim");
        aimDropdown.value = PlayerPrefs.GetInt("MouseAim");
        enemyCount = enemyParent.childCount;
        enemyCountText.text = "Enemies Remaining: " + enemyCount;

        foreach(Image i in ammoEggs)
        {
            i.material.SetTexture("_DecorTex", textures[Random.Range(0, textures.Length - 1)]);
        }
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

    public void UpdateAmmo(int amount)
    {
        eggIndex = Mathf.Min(amount, 12);
        ammoText.text = "Ammo: " + amount;
        for (int i = 0; i < 12; i++)
        {
            int index = i;
            if (index >= eggIndex)
                ammoEggs[index].gameObject.SetActive(false);
            else
            {
                if (!ammoEggs[index].gameObject.activeSelf)
                {
                    ammoEggs[index].material.SetTexture("_DecorTex", textures[Random.Range(0, textures.Length - 1)]);
                    ammoEggs[index].gameObject.SetActive(true);
                }
            }
        }
    }

    public void UpdateGold(int amount)
    {
        medalsText.text = "Medals: " + amount;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        player.tr.enabled = false;
        foreach (Transform de in deadEnemiesParent)
            de.GetComponent<Enemy>().tr.enabled = false;
        SceneManager.LoadScene(0);
    }

    public void Win()
    {
        CloseSettings();
        winScreen.SetActive(true);
        if(!player.win.isPlaying)
            player.win.Play();
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
        Time.timeScale = 1;
        ChangeCharacter();
    }

    public void ChangeCharacter()
    {
        player.playerMat.material.mainTexture = characters[characterIndex].texture;
        player.anim.Play(movements[characterIndex] + "Movement");
    }

    public Texture GetEggTexture(int ammo)
    {
        if(ammo > ammoEggs.Length)
        {
            return textures[Random.Range(0, textures.Length - 1)];
        }
        else
            return ammoEggs[ammo-1].material.GetTexture("_DecorTex");
    }

    void ChangeTarget()
    {
        if(enemyParent.childCount > 0)
            theCompass.target = enemyParent.GetChild(Random.Range(0, enemyParent.childCount - 1));
    }

    public void ShowCredits()
    {
        settings.SetActive(false);
        resourcesView.SetActive(false);
        creditsPage.SetActive(true);
    }
    public void CloseCredits()
    {
        settings.SetActive(true);
        creditsPage.SetActive(false);
        resourcesView.SetActive(false);
    }

    public void ShowArmoredIcon()
    {
        armoredIcon.SetActive(true);
    }
    public void HideArmoredIcon()
    {
        armoredIcon.SetActive(false);
    }

    public void ToggleMute()
    {
        if (mute.isOn == false)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);

    }

    public void ShowResources()
    {
        creditsPage.SetActive(false);
        resourcesView.SetActive(true);
    }


    public void ShowHowToPlay()
    {
        settings.SetActive(false);
        howToPlayPage.SetActive(true);
    }
    public void CloseHowToPlay()
    {
        settings.SetActive(true);
        howToPlayPage.SetActive(false);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
