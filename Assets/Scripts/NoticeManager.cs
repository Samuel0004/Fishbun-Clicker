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
                offset = "��� �ؾ�� �ر��ϴ� ���� ��ǥ.";
                break;
            case "Clear":
                offset = "��� �ؾ�� �ر��߾��!";
                break;
            case "Sell":
                offset = "�ؾ�� �巡���ؼ� �ָӴϿ� ���� �� �� �־��.";
                break;
            case "NotJelatin":
                offset = "���� �����մϴ�.";
                break;
            case "NotGold":
                offset = "��尡 �����մϴ�.";
                break;
            case "NotNum":
                offset = "�ؾ ���뷮�� �����մϴ�.";
                break;
            default:
                offset = "�ؾƻ� ���뷮�� �����մϴ�.";
                break;
        }
        uiController.Change(offset,isNegative);
    } 
}
