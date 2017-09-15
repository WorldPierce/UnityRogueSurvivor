using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playerTurn = true;

    private int level = 1;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;
    private List<Enemy> enemies;
    private bool enemiesMoving;


	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
	}

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }



    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        Invoke("HideLevelImage", levelStartDelay);

        levelImage.SetActive(true);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved...";
        levelImage.SetActive(true);
        enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(playerTurn || enemiesMoving ||doingSetup)
        {
            return;
        }
        StartCoroutine(MoveEnemies());

	}

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        } 

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;

    }
    /*
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }
    
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        instance.level++;
        instance.InitGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    */
}
