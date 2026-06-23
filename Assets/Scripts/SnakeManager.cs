using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SnakeManager : MonoBehaviour
{
    [InspectorName("Map Settings")]
    [SerializeField] float mapHeight;
    [SerializeField] float mapWidth;

    [Space(10)]
    [InspectorName("Prefabs")]

    [SerializeField] GameObject snakeHead;
    [SerializeField] GameObject snakeBodyPrefab;
    //[SerializeField] GameObject foodPrefab;
    [SerializeField] List<Food> foodList;
    [SerializeField] GameObject foodParent;

    [Space(10)]
    [InspectorName("Settings")]
    [SerializeField] bool spawnFood;
    [SerializeField] GameDifficulty gameDifficulty;

    public static SnakeManager Instance;
    public int score { get; private set; }

    private List<GameObject> snakeBodyList;

    public static event Action OnIncrementScore;
    public static event Action OnGameLossEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        snakeBodyList = new List<GameObject>();
        foodList = gameDifficulty.foodToSpawn;
        score = 0;

        if(spawnFood)
            InvokeRepeating("SpawnFood", 0f, 3f);

        //Debug.Log(gameDifficulty.difficultySettings);
    }

    void Update()
    {

    }

    public void AddSnakeBody()
    {
        Vector3 spawnPosition;
        Quaternion spawnQuaternion;

        if (snakeBodyList.Count == 0)
        {
            spawnPosition = snakeHead.transform.position - (snakeHead.transform.up * 1.1f);
            spawnQuaternion = snakeHead.transform.rotation;
        }
        else
        {
            GameObject snakeBody = snakeBodyList[snakeBodyList.Count - 1];
            spawnPosition = snakeBody.transform.position - (snakeBody.transform.up * 1.1f);
            spawnQuaternion = snakeBody.transform.rotation;
        }

        GameObject newSnakeBody = Instantiate(snakeBodyPrefab, spawnPosition, spawnQuaternion, gameObject.transform);
        newSnakeBody.name += $"_{snakeBodyList.Count}";
        snakeBodyList.Add(newSnakeBody);
        SetTargetFollowOnNewObject(newSnakeBody);
    }

    void SetTargetFollowOnNewObject(GameObject newSnakeBody)
    {
        int index = snakeBodyList.IndexOf(newSnakeBody);

        if(index == 0)
        {
            newSnakeBody.GetComponent<SnakeFollow>().targetToFollow = snakeHead;
        }
        else
        {
            GameObject bodyToFollow = snakeBodyList[index - 1];
            //Debug.Log($"Object in front: {bodyToFollow.name}");
            newSnakeBody.GetComponent<SnakeFollow>().targetToFollow = bodyToFollow;
        }
    }

    public GameObject GetSnakeBodyInFront(GameObject go)
    {
        if (snakeBodyList.Count <= 1)
            return snakeHead;

        int index = snakeBodyList.IndexOf(go);

        //Debug.Log($"Object in front: {snakeBodyList[index - 1].name}");
        return snakeBodyList[index - 1];
    }

    void SpawnFood()
    {
        int i, attemptsLeft = foodList.Count;
        do
        {
            i = UnityEngine.Random.Range(0, foodList.Count);
            attemptsLeft--;
        } while (!PickRandomPosAndSpawnFood(foodList[i]) && attemptsLeft > 0);
    }

    bool PickRandomPosAndSpawnFood(Food foodObject)
    {
        int count = foodParent.transform.Cast<Transform>().Count(c => c.CompareTag(foodObject.foodPrefab.tag));
        if (foodObject.maxToSpawn != 0 && foodObject.maxToSpawn <= count)
            return false;

        float xSpawn = UnityEngine.Random.Range(-mapWidth / 2 + 2, mapWidth / 2 - 2);
        float zSpawn = UnityEngine.Random.Range(-mapHeight / 2 + 2, mapHeight / 2 - 2);

        Instantiate(foodObject.foodPrefab, new Vector3(xSpawn, 0.55f, zSpawn), Quaternion.identity, foodParent.transform);

        return true;
    }

    public void IncrementScore()
    {
        score++;
        
        OnIncrementScore?.Invoke();
        //Debug.Log($"Score: {score}");
    }

    public float IncreaseSnakeSpeed(float currSpeed)
    {
        if (gameDifficulty.speedPercIncrease == 0 || gameDifficulty.speedIncreaseAfter == 0)
            return currSpeed;

        if (score > 0 && score % gameDifficulty.speedIncreaseAfter == 0)
            currSpeed += currSpeed * gameDifficulty.speedPercIncrease / 100;

        //Debug.Log($"New Speed: {currSpeed}");
        return currSpeed;
    }

    public void TriggerGameLoss()
    {
        OnGameLossEvent?.Invoke();
    }
}
