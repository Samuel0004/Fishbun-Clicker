using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantPannelUI : MonoBehaviour {
    [Header("PlantPannel")]
    [SerializeField] private Text numText;
    [SerializeField] private Text numGoldText;
    [SerializeField] private Text clickText;
    [SerializeField] private Text clickGoldText;
    [SerializeField] private GameObject centerText;
    [SerializeField] private Button numBtn;
    [SerializeField] private Button clickBtn;

    private void Awake() {
        numBtn.onClick.AddListener(() => {
            Num();
        });
        clickBtn.onClick.AddListener(() => {
            Click();
        });
    }

    private void Start() {
        UpdatePlantPannel();
    }
    private void UpdatePlantPannel() {
        NumGroupChange();
        ClickGroupChange();

    }

    private void NumGroupChange() {
        //change NUM GROUP

        if (GameManager.Instance.NumLevel >= 5)
            numBtn.gameObject.SetActive(false);


        //change the sub text in accordance to numlevel
        numGoldText.text = string.Format("{0:n0}", GameManager.Instance.GetNumGoldFromArray(GameManager.Instance.NumLevel));
        numText.text = string.Format("젤리 수용량 {0}", GameManager.Instance.NumLevel * 2);

    }
    private void ClickGroupChange() {
        if (GameManager.Instance.ClickLevel >= 5)
            clickBtn.gameObject.SetActive(false);

        //change CLICK GROUP 
        clickGoldText.text = string.Format("{0:n0}", GameManager.Instance.GetClickGoldFromArray(GameManager.Instance.ClickLevel));
        clickText.text = string.Format("클릭 생산량 {0}", GameManager.Instance.ClickLevel);
    }
    private void Num() {
        //if current gold is higher than req gold to unlock
        if (GameManager.Instance.Gold >= GameManager.Instance.GetNumGoldFromArray(GameManager.Instance.NumLevel)) {
            //subtract req cost from jelatine
            GameManager.Instance.SetGold(GameManager.Instance.Gold - GameManager.Instance.GetNumGoldFromArray(GameManager.Instance.NumLevel));
            GameManager.Instance.SetNumLevel(GameManager.Instance.NumLevel + 1);
            //play clip
            SoundManager.Instance.ChangeSfxClip("Unlock");
            UpdatePlantPannel();
            GameManager.Instance.Save();
        }
        else {
            ClipAndMessage("Fail", "NotGold");
        }
    }
    private void Click() {
        //if current gold is higher than req gold to unlock
        if (GameManager.Instance.Gold >= GameManager.Instance.GetClickGoldFromArray(GameManager.Instance.ClickLevel)) {
            //subtract req cost from jelatine
            GameManager.Instance.SetGold(GameManager.Instance.Gold - GameManager.Instance.GetClickGoldFromArray(GameManager.Instance.ClickLevel));
            GameManager.Instance.SetClickLevel(GameManager.Instance.ClickLevel + 1);
            //play clip
            SoundManager.Instance.ChangeSfxClip("Unlock");
            UpdatePlantPannel();
            GameManager.Instance.Save();
        }
        else {
            ClipAndMessage("Fail", "NotGold");
        }
    }
    private void ClipAndMessage(string clip, string message) {
        //function that takes two string as arguement to change both played lip and message
        SoundManager.Instance.ChangeSfxClip(clip);
        NoticeManager.Instance.Message(message);
    }
}