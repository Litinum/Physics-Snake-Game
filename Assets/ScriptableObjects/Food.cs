using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Scriptable Objects/Food")]
public class Food : ScriptableObject
{
    public GameObject foodPrefab;
    public int maxToSpawn;
    public float speed;
    public float turnSpeed;
}
