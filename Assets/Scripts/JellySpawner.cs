using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour {
    public static JellySpawner Instance {get; private set;}

    [HideInInspector]private JellyData[] jellyDatas;
    [SerializeField]public GameObject jellyPrefab;

    public event EventHandler<OnJellySpawnedEventArgs> OnJellySpawnedEvent;
    public class OnJellySpawnedEventArgs : EventArgs {
        public GameObject jelly;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    public void SpawnJelly(int page,int level, float exp)
    {
        //get respective scriptable object jelly asset 
        //JellyData jellyData = jellyDatas[page];

        //get a random point nad make an instance of the prefab
        GameObject instance = Instantiate(jellyPrefab, transform);

        Vector3 point = GameManager.Instance.GetPoint();
        instance.GetComponent<JellyMove>().InstantiateJelly(page, level, exp,point);

        OnJellySpawnedEvent?.Invoke(this, new OnJellySpawnedEventArgs { jelly = instance }) ;
    }

}
