using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] GameObject foodPrefab;

    [Space(10)]
    [InspectorName("Settings")]
    [SerializeField] bool spawnFood;

    public static SnakeManager Instance;
    public int score { get; private set; }

    private List<GameObject> snakeBodyList;

    public static event Action OnIncrementScore;

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
        score = 0;

        if(spawnFood)
            InvokeRepeating("SpawnFood", 0f, 3f);
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

        Debug.Log($"Object in front: {snakeBodyList[index - 1].name}");
        return snakeBodyList[index - 1];
    }

    void SpawnFood()
    {
        float xSpawn = UnityEngine.Random.Range(-mapWidth / 2 + 2, mapWidth / 2 - 2);
        float zSpawn = UnityEngine.Random.Range(-mapHeight / 2 + 2, mapHeight / 2 - 2);

        Instantiate(foodPrefab, new Vector3(xSpawn, 0.55f, zSpawn), Quaternion.identity);
    }

    public void IncrementScore()
    {
        score++;
        OnIncrementScore?.Invoke();
        //Debug.Log($"Score: {score}");
    }
}
