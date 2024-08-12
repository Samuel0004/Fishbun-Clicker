using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyPannelUI : MonoBehaviour {

    [SerializeField] private Image unlockImage;
    [SerializeField] private Text jellyText;
    [SerializeField] private Text goldText;
    [SerializeField] private Image lockImage;
    [SerializeField] private GameObject lockGroup;
    [SerializeField] private Text jelatinText;
    [SerializeField] private Text pageText;

    [SerializeField] private Button pageLeftButton;
    [SerializeField] private Button pageRightButton;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button buyButton;


    [SerializeField] private GameObject centerText;

    private void Awake() {
        pageLeftButton.onClick.AddListener(()=> {
            PageDown();
        });
        pageRightButton.onClick.AddListener(() => {
            PageUp();
        });
        unlockButton.onClick.AddListener(() => {
            Unlock();
        });
        buyButton.onClick.AddListener(() => {
            Buy();
        });

    }

    private void Start() {
        UpdateJellyPannel();   
    }

    private void UpdateJellyPannel() {
        LockGroupChange();
        UnlockGroupChange();
    }
    private void LockGroupChange() {
        int page = UIController.Instance.GetPage();

        //change sprite and set native size
        unlockImage.sprite = GameManager.Instance.GetSpriteFromArray(page);
        unlockImage.SetNativeSize();

        //change jellyname
        jellyText.text = GameManager.Instance.GetNameFromArray(page);
        //change jelly gold and page text
        goldText.text = string.Format("{0:n0}", GameManager.Instance.GetGoldFromArray(page)); //format string: comma for 1000, with no decimal
        pageText.text = string.Format("#{0:00}", page + 1); //format string: always keep ##, except show 0s
    }

    private void UnlockGroupChange() {
        int page = UIController.Instance.GetPage();

        //set active if not unlocked
        bool unlocked = GameManager.Instance.GetUnlockFromArray(page);
        lockGroup.SetActive(!unlocked);

        //change sprite and set native size
        lockImage.sprite = GameManager.Instance.GetSpriteFromArray(page);
        lockImage.SetNativeSize();

        //change jelly jelatin and page text
        jelatinText.text = string.Format("{0:n0}", GameManager.Instance.GetJelatinFromArray(page)); //format string: comma for 1000, with no decimal
        pageText.text = string.Format("#{0:00}", page + 1); //format string: always keep ##, except show 0s
    }
    private void Unlock() {
        int page = UIController.Instance.GetPage();

        //if current jelatine is higher than req jelatin to unlock
        if (GameManager.Instance.Jelatine >= GameManager.Instance.GetJelatinFromArray(page)) {
            //subtract req cost from jelatine
            GameManager.Instance.SetJelatine(GameManager.Instance.Jelatine - GameManager.Instance.GetJelatinFromArray(page));
            GameManager.Instance.SetUnlockInArray(page);

            //check if all items are unlocked JellyUnlockedList 
            foreach (bool x in GameManager.Instance.GetUnlockArray()) {
                //if there is an item, which is not unlocked
                if (x == false) {
                    //play unlocked clip
                    SoundManager.Instance.ChangeSfxClip("Unlock");
                    UpdateJellyPannel();
                    return;
                }
            }
            //else play clear clip and show clear message
            ClipAndMessage("Clear", "Clear");
            GameManager.Instance.SetClearGame();
            UIController.Instance.UpdateMedal();
            UpdateJellyPannel();
        }
        else {
            ClipAndMessage("Fail", "NotJelatin");
        }
        GameManager.Instance.Save();
    }

    private void Buy() {
        int page = UIController.Instance.GetPage();

        //if current gold is higher than req gold to unlock
        if (GameManager.Instance.Gold >= GameManager.Instance.GetGoldFromArray(page) && GameManager.Instance.GetJellyList().Count < GameManager.Instance.NumLevel * 2) {
            //subtract req cost from jelatine
            GameManager.Instance.SetGold(GameManager.Instance.Gold - GameManager.Instance.GetGoldFromArray(page));
            JellySpawner.Instance.SpawnJelly(page, 1, 0f);
            UpdateJellyPannel();
        }
        else {
            //not enough gold
            if (GameManager.Instance.Gold < GameManager.Instance.GetGoldFromArray(page)) {
                ClipAndMessage("Fail", "NotGold");
            }
            //not enough num 
            else {
                ClipAndMessage("Fail", "NotNum");
            }
        }
    }
    public void PageUp() {
        if (UIController.Instance.GetPage() != 11) {
            UIController.Instance.IncrementPage();
            //play clip
            SoundManager.Instance.ChangeSfxClip("Button");
            UpdateJellyPannel();
        }
    }
    public void PageDown() {
        if (UIController.Instance.GetPage() != 0) {
            UIController.Instance.DecrementPage();
            //play clip
            SoundManager.Instance.ChangeSfxClip("Button");
            UpdateJellyPannel();
        }
    }
    private void ClipAndMessage(string clip, string message) {
        //function that takes two string as arguement to change both played lip and message
        SoundManager.Instance.ChangeSfxClip(clip);
        NoticeManager.Instance.Message(message);
    }
}