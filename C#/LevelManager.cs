using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    
    public List<LevelDefinition> levelDefinitions;
    public List<Level> levels;
    public int currentTokensCollected = 0;
    public int currentLevelInt = 0;
    [SerializeField] private GameObject frogTokenObject;
    public Transform objectPoolerParent;
    [SerializeField] private CinemachineConfiner virtualCamConfiner;

    [Header("Audio")]

#region Singleton
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion//This just makes the script accessible anywhere in the game, since there's only 1 of this script in the game.

    public void OnPlayButtonPress()
    {
        UIManager.instance.mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    StartGame();
    }

    public void StartGame() {

        CameraClamp.camInstance.gameIsStarted = true;
        virtualCamConfiner.m_BoundingVolume = levels[currentLevelInt].confinerCollider;
        frogTokenObject.SetActive(true);

        currentTokensCollected = 0;
        ClearEnemies();
        UIManager.instance.levelSelectionPanel.SetActive(false);
        UIManager.instance.inGamePanel.SetActive(true);
        UIManager.instance.UpdateCurrentScore();
        PlayerManager.instance.SetPlayersActive();
      


        Time.timeScale = 1.25f;

        EnemyManager.instance.StartSpawning();

        PlayerManager.instance.SetStatsToAllPlayers();
        UIManager.instance.InitializePlayersLives();
        PlayerManager.instance.OnGameStart();
        MoveToken(false);

        CameraSetUp();
        LoadLevel();
    }

    int lastLocation = 0;
    public void MoveToken(bool _playerTouched)
    {
        if(_playerTouched)
        ObjectPooler.Instance.SpawnFromPool("TokenParticles", frogTokenObject.transform.position, Quaternion.Euler(Vector3.zero));

        int randomSpawnInt = Random.Range(0, levels[currentLevelInt].tokenSpawnPoint.Count);
        if (randomSpawnInt == lastLocation) { MoveToken(false);  return; }
        frogTokenObject.transform.position = levels[currentLevelInt].tokenSpawnPoint[randomSpawnInt].position;
        lastLocation = randomSpawnInt;
    }

    public Vector3 RandomPlayerSpawnPoint() {

        int randomSpawnInt = Random.Range(0, levels[currentLevelInt].playerSpawnPoint.Count);
        return levels[currentLevelInt].playerSpawnPoint[randomSpawnInt].transform.position;
    }

    public void GameOver() {
        UIManager.instance.lastScore.text = currentTokensCollected.ToString();
        CheckForHighscore();
        UIManager.instance.highScore.text = PlayerManager.instance.highScores[currentLevelInt].ToString();
        EnemyManager.instance.EndSpawning();
        CameraClamp.camInstance.gameIsStarted = false;
    }

    public void CheckForHighscore() {
        if (currentTokensCollected > PlayerManager.instance.highScores[currentLevelInt]) {
            PlayerManager.instance.highScores[currentLevelInt] = currentTokensCollected;
        }
    }

    public void CameraSetUp()
    {
        Vector2 _minXY = new Vector2(levelDefinitions[currentLevelInt].cameraMin.x, levelDefinitions[currentLevelInt].cameraMin.y);
        Vector2 _maxXY = new Vector2(levelDefinitions[currentLevelInt].cameraMax.x, levelDefinitions[currentLevelInt].cameraMax.y);

        CameraClamp.camInstance.UpdateCameraTargets();
        CameraClamp.camInstance.SetClamps(_minXY,_maxXY);
    }

    public float CurrentLevelMultiplier()
    {
        return levelDefinitions[currentLevelInt].levelMultiplier;
    }

    public Vector3 GetRandomEnemySpawnPoint()
    {
        Vector3 enemySpawnPoint = Vector3.zero;
        int spawnLocationInt = Random.Range(0, levels[currentLevelInt].enemySpawnPoint.Count);
        return levels[currentLevelInt].enemySpawnPoint[spawnLocationInt].transform.position;
    }


    public void ClearEnemies()
    {
        foreach(Transform enemy in objectPoolerParent)
        {
            if (enemy.gameObject.tag == "enemy")
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void LoadLevel()
    {
        for(int i = 0; i < levels.Count; i++)
        {
            if (i == currentLevelInt)
            {
                levels[i].levelObject.SetActive(true);

            }
            else
            {
                levels[i].levelObject.SetActive(false);
            }
        }
    }

    public void DisableAllPooledObjects()
    {
        foreach (Transform obj in objectPoolerParent)
        {

            obj.gameObject.SetActive(false);
            
        }
        frogTokenObject.SetActive(false);

    }

    public LevelDefinition GetLevelDefinition()//This is only used in UIManager OnNextLevelButton function
    {
    
            return levelDefinitions[currentLevelInt];
        }

    
    public int GetFurthestLevel()
    {
        int i = 0;
        bool beenfound = false;
        List<int> _highscores = PlayerManager.instance.highScores;
        List<int> _scoreRequirements = GetScoreRequirements();
        foreach(int score in _highscores) {

            if (!beenfound)
            {

                if (score> _scoreRequirements[i])
                {
                    return i;
                }

                    i++;
            }
        }
        int intToReturn = i;
        
        return intToReturn;
    }

    public List<int> GetScoreRequirements()
    {
        List<int> _scoreReqs = new List<int>();

        foreach(LevelDefinition _levelDef in levelDefinitions)
        {
            _scoreReqs.Add(_levelDef.scoreRequirement);
        }

        return _scoreReqs;
    }

    public int GetHighestScoreAtFurthestLevel(int _furthestLevel)
    {
        return PlayerManager.instance.highScores[_furthestLevel];
    }
}

[System.Serializable]
public class Level
{
    public GameObject levelObject;
    public Collider confinerCollider;
    public List<Transform> tokenSpawnPoint;
    public List<Transform> playerSpawnPoint;
    public List<Transform> enemySpawnPoint;
    public List<GameObject> enemy;
    
}