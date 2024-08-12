using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : MonoBehaviour
{
    public static NoticeManager Instance { get; private set; }

    private bool isNegative;
    public UIController uiController;
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        uiController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
    }
    public void Message(string message)
    {
        string offset;
        isNegative = Equals("Not",message.Substring(0, 3));

        switch (message)
        {
            case "Start":
                offset = "모든 붕어빵을 해금하는 것이 목표.";
                break;
            case "Clear":
                offset = "모든 붕어빵을 해금했어요!";
                break;
            case "Sell":
                offset = "붕어빵을 드래그해서 주머니에 놓아 팔 수 있어요.";
                break;
            case "NotJelatin":
                offset = "밭이 부족합니다.";
                break;
            case "NotGold":
                offset = "골드가 부족합니다.";
                break;
            case "NotNum":
                offset = "붕어빵 수용량이 부족합니다.";
                break;
            default:
                offset = "붕아빵 수용량이 부족합니다.";
                break;
        }
        uiController.Change(offset,isNegative);
    } 
}
