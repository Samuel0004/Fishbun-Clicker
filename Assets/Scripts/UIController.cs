using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //create singleton
    public static UIController Instance { get; private set; }

    //variables to control UI
    private bool isLive;
    private int page;
    private bool pannelDown;

    [SerializeField] private GameObject centerText;

    private Coroutine a;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        isLive = true;
        page = 0;
    }

    private void Start()
    {
        UpdateMedal();
    }

    public void UpdateMedal()
    {
        GameObject Medal = transform.Find("Medal").gameObject;
        if (Medal != null)
        {
            Medal.SetActive(GameManager.Instance.ClearGame);
        }
    }

    
    
    public void Change(string text, bool isNegative)
    {
        Text noticeText = centerText.transform.Find("Text").GetComponent<Text>();
        Image image = centerText.GetComponent<Image>();
       
        //if notice is a negative notice
        if (isNegative)
        {
            //change color, position, and text
            image.color = new Color32(255, 97, 97, 255);
            noticeText.text = text;
        }
        else
        {
            image.color = new Color32(0, 200, 187, 255);
            noticeText.text = text;
        }
        
        //Stop previous coroutine and begin a new one
        if(a!=null)StopCoroutine(a);
        centerText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3);
        a= StartCoroutine(Timer());
    }

    public bool GetLive(){
        return isLive;
    }
    public void SetLiveTrue( ) {
        isLive = true;
    }
    public void SetLiveFalse() {
        isLive = false;
    }
    public int GetPage() {
        return page;
    }
    public void IncrementPage() {
        page++;
    }
    public void DecrementPage() {
        page--;
    }
    public bool GetPannelDown() {
        return pannelDown;
    }

    public void SetPannelDown(bool offset) {
        pannelDown = offset;
    }
    IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(7f);
        centerText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
    }
}
