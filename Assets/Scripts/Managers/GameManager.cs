using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
 
            return _instance;
        }
    }


    [Header("Game Objects")] 
    [ReadOnly] public PlayerGroup playerGroup;
    [ReadOnly] public CameraMovement cameraMovement;
    
    private void Awake()
    {
        _instance = this;
        FindObjects();
    }

    public void FindObjects()
    {
        playerGroup = FindObjectOfType<PlayerGroup>();
    }


    public void Win()
    {
        //gold hesaplama
    }

    public void Lose()
    {
        playerGroup.speed = Vector2.zero;
    }
}
