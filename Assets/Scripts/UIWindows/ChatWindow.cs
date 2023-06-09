using SangoCommon.Classs;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The AvaterInfoWindow.

public class ChatWindow : BaseWindow
{
    public ScrollRect scrollRect;

    public RectTransform chatContentRectTransform;
    public Transform chatBubbleRoot;

    public TMP_InputField chatMessageInputField;

    private float maxChatTextWidth = 300;
    private float profileImageWidth = 60;
    private float rightProfileImageCenter = 450;
    private int maxAllChatBubbleNumber = 30;

    private Queue<GameObject> allAccountChatBubbleQueue = new Queue<GameObject>();

    protected override void InitWindow()
    {
        base.InitWindow();

        chatContentRectTransform.sizeDelta = new Vector2(chatContentRectTransform.rect.width, 30);
    }

    protected override void ClearWindow()
    {
        base.ClearWindow();
        for (int i = 0; i < chatContentRectTransform.childCount; i++)
        {
            Destroy(chatContentRectTransform.GetChild(i).gameObject);
        }
        allAccountChatBubbleQueue.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameManager.Instance.GameMode == GameModeCode.ChatMode)
            {
                ChatSystem.Instance.SendChatMessage(chatMessageInputField.text);
            }
            else
            {
                SangoRoot.AddMessage("亲，你输入太快了，服务器君没有反应过来呢", TextColorCode.RedColor);
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (GameManager.Instance.GameMode == GameModeCode.ChatMode)
            {
                OnlineAccountChatMessage testMessage = new OnlineAccountChatMessage
                {
                    Account = "Test",
                    Message = "这是模拟在线用户专用的语句，当你按下F1键时，就会自动出现这句话"
                };
                SetOnlineAccountChatBubbleMessage(testMessage);
            }            
        }
    }

    public void SetOnlineAccountChatBubbleMessage(OnlineAccountChatMessage onlineAccountChatMessage)
    {
        GameObject onlineAccountChatBubble = (GameObject)Instantiate(Resources.Load(ChatUIConstant.OnlineAccountChatBubblePath));
        onlineAccountChatBubble.transform.SetParent(chatBubbleRoot);
        TMP_Text onlineAccountChatBubbleText = onlineAccountChatBubble.transform.Find("ChatBG").GetComponentInChildren<TMP_Text>();
        Image onlineAccountChatBubbleImage = onlineAccountChatBubble.transform.Find("ChatBG").GetComponent<Image>();
        TMP_Text onlineAccountProfileId = onlineAccountChatBubble.transform.Find("ProfileImage").GetComponentInChildren<TMP_Text>();
        onlineAccountProfileId.text = onlineAccountChatMessage.Account;
        onlineAccountChatBubbleText.text = onlineAccountChatMessage.Message;
        if (onlineAccountChatBubbleText.preferredWidth > maxChatTextWidth)
        {
            onlineAccountChatBubbleText.GetComponent<LayoutElement>().preferredWidth = maxChatTextWidth;
        }

        Vector2 selfFitChatBubbleSize = GetContentSizeFitterPrefferSize(onlineAccountChatBubbleImage.GetComponent<RectTransform>(), onlineAccountChatBubbleImage.GetComponent<ContentSizeFitter>());
        onlineAccountChatBubble.GetComponent<RectTransform>().sizeDelta = new Vector2(100, selfFitChatBubbleSize.y);
        onlineAccountChatBubbleImage.transform.localPosition = new Vector2(profileImageWidth / 2 + selfFitChatBubbleSize.x / 2, onlineAccountChatBubbleImage.transform.localPosition.y);
        chatContentRectTransform.sizeDelta = new Vector2(chatContentRectTransform.rect.width, chatContentRectTransform.rect.height + selfFitChatBubbleSize.y);
        allAccountChatBubbleQueue.Enqueue(onlineAccountChatBubble);
        AdjustScrollChatContent();
    }

    public void SetLocalAccountChatBubbleMessage(OnlineAccountChatMessage onlineAccountChatMessage)
    {
        GameObject localAccountChatBubble = (GameObject)Instantiate(Resources.Load(ChatUIConstant.LocalAccountChatBubblePath));
        localAccountChatBubble.transform.SetParent(chatBubbleRoot);
        TMP_Text localAccountChatBubbleShowsText = localAccountChatBubble.transform.Find("ChatBG").GetComponentInChildren<TMP_Text>();
        Image localAccountChatBubbleShowsImage = localAccountChatBubble.transform.Find("ChatBG").GetComponent<Image>();
        TMP_Text onlineAccountProfileId = localAccountChatBubble.transform.Find("ProfileImage").GetComponentInChildren<TMP_Text>();
        onlineAccountProfileId.text = onlineAccountChatMessage.Account;
        localAccountChatBubbleShowsText.text = onlineAccountChatMessage.Message;
        if (localAccountChatBubbleShowsText.preferredWidth > maxChatTextWidth)
        {
            localAccountChatBubbleShowsText.GetComponent<LayoutElement>().preferredWidth = maxChatTextWidth;
        }

        Vector2 selfFitChatBubbleSize = GetContentSizeFitterPrefferSize(localAccountChatBubbleShowsImage.GetComponent<RectTransform>(), localAccountChatBubbleShowsImage.GetComponent<ContentSizeFitter>());
        localAccountChatBubble.GetComponent<RectTransform>().sizeDelta = new Vector2(100, selfFitChatBubbleSize.y);
        localAccountChatBubbleShowsImage.transform.localPosition = new Vector2(rightProfileImageCenter - profileImageWidth / 2 - selfFitChatBubbleSize.x / 2, localAccountChatBubbleShowsImage.transform.localPosition.y);
        chatContentRectTransform.sizeDelta = new Vector2(chatContentRectTransform.rect.width, chatContentRectTransform.rect.height + selfFitChatBubbleSize.y);
        allAccountChatBubbleQueue.Enqueue(localAccountChatBubble);
        AdjustScrollChatContent();
    }

    private void AdjustScrollChatContent()
    {
        if (allAccountChatBubbleQueue.Count > maxAllChatBubbleNumber)
        {
            GameObject oldAccountChatBubbleShows = allAccountChatBubbleQueue.Dequeue();
            chatContentRectTransform.sizeDelta = new Vector2(chatContentRectTransform.rect.width, chatContentRectTransform.rect.height - oldAccountChatBubbleShows.GetComponent<RectTransform>().rect.height);
            Destroy(oldAccountChatBubbleShows);
        }
        scrollRect.verticalNormalizedPosition = 0;
    }

    private Vector2 GetContentSizeFitterPrefferSize(RectTransform rectTransform, ContentSizeFitter contentSizeFitter)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        return new Vector2(HandleSelfFitAlongAxis(0, rectTransform, contentSizeFitter), HandleSelfFitAlongAxis(1, rectTransform, contentSizeFitter));
    }

    private float HandleSelfFitAlongAxis(int axis, RectTransform rectTransform, ContentSizeFitter contentSizeFitter)
    {
        ContentSizeFitter.FitMode fitMode;
        if (axis == 0)
        {
            fitMode = contentSizeFitter.horizontalFit;
        }
        else
        {
            fitMode = contentSizeFitter.verticalFit;
        }
        return LayoutUtility.GetPreferredSize(rectTransform, axis);
    }
}
