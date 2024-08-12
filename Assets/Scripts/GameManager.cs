using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //all lists
    [SerializeField] private int[] jellyJelatinList;
    [SerializeField] private int[] jellyGoldList;
    [SerializeField] private int[] numGoldList;
    [SerializeField] private int[] clickGoldList;
    [SerializeField] private Vector3[] PointList;
    [SerializeField] private Sprite[] beanSpriteArray;
    [SerializeField] private string[] beanNameList;
    [SerializeField] private bool[] jellyUnlockList;
    [SerializeField] private RuntimeAnimatorController[] LevelAc;

    [SerializeField] private Text JelatineText;
    [SerializeField] private Text GoldText;

    [SerializeField]private List<GameObject> jellyList;

    List<SaveObject> entries = new List<SaveObject>();

    //saved var
    public bool IsSell;
    public int Jelatine { get; private set; }
    public int Gold { get; private set; }
    public bool isLoading { get; private set; }
    public int NumLevel { get; private set; }
    public int ClickLevel { get; private set; }
    public bool ClearGame { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        JellySpawner.Instance.OnJellySpawnedEvent += JellySpawner_OnJellySpawnedEvent;
        jellyList = new List<GameObject>();

        FileHandler.Init();

        StartGame();
    }

    private void JellySpawner_OnJellySpawnedEvent(object sender, JellySpawner.OnJellySpawnedEventArgs e) {
        jellyList.Add(e.jelly);
        if (!isLoading) { 
            SoundManager.Instance.ChangeSfxClip("Buy");
            Save();
        }
    }


    public void CheckClear()
    {
        if (ClearGame)
        {
            NoticeManager.Instance.Message("Clear");
        }
        else
            NoticeManager.Instance.Message("Start");
    }

    IEnumerator AutoGetJelly()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            foreach(GameObject jelly in jellyList)
            {
                jelly.GetComponent<JellyMove>().GetJelatin();
            }
            yield return new WaitForSeconds(3f);
        }
        
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.E)) {
            //Jelatine += 100;
            //Gold += 100;
        }
        //JelatineText Format and update
        JelatineText.text = string.Format("{0:n0}", Jelatine);

        //GoldText Format and Update
        GoldText.text = string.Format("{0:n0}", Gold);
    }

    public void ChangeAc(Animator anim, int level)
    {
        //change the animation in runtime 
        anim.runtimeAnimatorController = LevelAc[level - 1];
    }

    public Vector3 GetPoint()
    {
        //Calculate new speed x and y
        Vector3 Point = PointList[Random.Range(0, PointList.Length)]; //Picks a random point from the list
        return Point;
    }
    public int GetJelatinFromArray(int id) {
        return jellyJelatinList[id];
    }
    public bool GetUnlockFromArray(int id) {
        return jellyUnlockList[id];
    }

    public bool[] GetUnlockArray() {
        return jellyUnlockList;
    }
    public void SetUnlockInArray(int id) {
        jellyUnlockList[id] = true;
    }
    public int GetGoldFromArray(int id) {
        return jellyGoldList[id];
    }
    public string GetNameFromArray(int id) {
        return beanNameList[id];
    }
    public Sprite GetSpriteFromArray(int id) {
        return beanSpriteArray[id];
    }
    public int GetClickGoldFromArray(int id) {
        return clickGoldList[id];
    }

    public int GetNumGoldFromArray(int id) {
        return numGoldList[id];
    }
    public void RemoveJelly(JellyMove jelly) {
        jellyList.Remove(jelly.gameObject);
        Destroy(jelly.gameObject);
        Save();
    }

    public List<GameObject> GetJellyList() {
        return jellyList;
    }

    public void SetNumLevel(int numLevel) {
        NumLevel = numLevel;
    }
    public void SetClickLevel(int clickLevel) {
        ClickLevel = clickLevel;
    }

    public void SetGold(int gold) {
        Gold = gold;
    }
    public void SetJelatine(int jelatine) {
        Jelatine = jelatine;
    }
    public void SetClearGame() {
        ClearGame = true;
    }
    public void Save()
    {
        //Save
        //if loading prevent save so it doesnt save each time jelly is added
        if (isLoading)
            return;

        //create a class object to contain data only 
        int i = 0;
        int[] id = new int[jellyList.Count];
        int[] level =new int[jellyList.Count];
        float[] exp = new float[jellyList.Count];
        foreach (GameObject jelly in jellyList)
        {
            id[i] = jelly.GetComponent<JellyMove>().GetJellyId();
            level[i] = jelly.GetComponent<JellyMove>().GetJellyLevel();
            exp[i] = jelly.GetComponent<JellyMove>().GetJellyExp();
            i++;
        }

        //Create new instance of saveObject 
        SaveObject saveObject = new SaveObject
        {
            goldAmount = Gold,
            jelatineAmount = Jelatine,
            tempJellyList = jellyList,
            unlockList = jellyUnlockList,
            idSave = id,
            levelSave = level,
            expSave = exp,
            numLevel = NumLevel,
            clickLevel = ClickLevel,
            isClear = ClearGame
        };

        entries.Clear();
        entries.Add(saveObject);

        FileHandler.SaveToJson<SaveObject>(entries);

    }
    private void StartGame() {
        Load();

        CheckClear();
    }
    private void Load()
    {
        isLoading = true;

        entries = FileHandler.ReadFromJson<SaveObject>("save_");

        // Load Data to Card and Dice Manager
        if (entries.Count == 1) {
            SaveObject saveObject = entries[0];

            Gold = saveObject.goldAmount;
            Jelatine = saveObject.jelatineAmount;
            jellyUnlockList = saveObject.unlockList;
            NumLevel = saveObject.numLevel;
            ClickLevel = saveObject.clickLevel;
            ClearGame = saveObject.isClear;

            for(int i = 0; i < saveObject.tempJellyList.Count; i++) {
                JellySpawner.Instance.SpawnJelly(saveObject.idSave[i], saveObject.levelSave[i], saveObject.expSave[i]);
            }
          
        }
        else {
            // New Game
            NumLevel = 1;
            ClickLevel = 1;
            Gold = 100;
            Jelatine = 100;
            SoundManager.Instance.SetSFXVolume(0.5f);
            SoundManager.Instance.SetBGMVolume(0.5f);
        }
        isLoading = false;
        StartCoroutine(AutoGetJelly());
    }
}
