using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //list of enemies
    private List<Enemy> enemies = new();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addEnemy(Enemy e) 
    {
        enemies.Add(e);
    }

    public void MakeNoise(Vector2 position, float radius) 
    {
        foreach (Enemy e in enemies)
        {
            float dist = (position - (Vector2)e.transform.position).magnitude;
            if (dist <= radius) e.Notify(position);
        }   
    }
}
