using Assets.Scripts.Common.Constant;
using SangoCommon.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Dynamic Window.

public class DynamicWindow : BaseWindow
{
    public Animation messageShowAnimation;
    public TMP_Text messageText;
    public Transform uiItemRoot;

    private bool isMessageShow = false;
    private Queue<string> messageQueue = new Queue<string>();

    private Dictionary<string, EnemyHpUIShows> enemyHpUIShowsDict = new Dictionary<string, EnemyHpUIShows>();
    private List<ElementReactionNameShows> elementReactionShowsList = new List<ElementReactionNameShows>();
    private List<AttackNumShows> attackNumShowsList = new List<AttackNumShows>();

    private float scaleRate = 1.0f * 1080 / Screen.height;
    private Vector3 enemyHeight;

    protected override void InitWindow()
    {
        base.InitWindow();
        SetActive(messageText, false);
        enemyHeight = new Vector3(0, 400, 0);
    }

    public void AddMessage(string message)
    {
        lock (messageQueue)
        {
            messageQueue.Enqueue(message);
        }
    }

    public void AddEnemyHpUI(string enemyName, int hpFull, Vector3 position)
    {
        EnemyHpUIShows enemyHpUIShows = GameObjectPools<EnemyHpUIShows>.GetObject();
        enemyHpUIShows.hpUI.transform.position = Camera.main.WorldToScreenPoint(position);
        enemyHpUIShows.hpUI.transform.SetParent(uiItemRoot);
        enemyHpUIShows.hpUI.GetComponent<EnemyHpUIItem>().InitItem(hpFull);
        enemyHpUIShows.hpUI.SetActive(true);
        enemyHpUIShowsDict.Add(enemyName, enemyHpUIShows);
    }

    public void UpdateEnemyHpUI(string enemyName, int hp, Vector3 position)
    {
        if (enemyHpUIShowsDict.ContainsKey(enemyName))
        {
            enemyHpUIShowsDict[enemyName].hpUI.transform.position = Camera.main.WorldToScreenPoint(position);
            enemyHpUIShowsDict[enemyName].hpUI.GetComponent<EnemyHpUIItem>().SetHPFG(hp);
            enemyHpUIShowsDict[enemyName].hpUI.GetComponent<EnemyHpUIItem>().SetIsFocusEnemy();
        }
    }

    public void RemoveEnemyHpUI(string enemyName)
    {
        if (enemyHpUIShowsDict.ContainsKey(enemyName))
        {
            enemyHpUIShowsDict[enemyName].hpUI.SetActive(false);
            GameObjectPools<EnemyHpUIShows>.RecyclePool(enemyHpUIShowsDict[enemyName]);
            enemyHpUIShowsDict.Remove(enemyName);
        }
    }

    public void AddEnemyElementImg(string enemyName, ElementTypeCode elementType)
    {
        if (enemyHpUIShowsDict.ContainsKey(enemyName))
        {
            enemyHpUIShowsDict[enemyName].hpUI.GetComponent<EnemyHpUIItem>().SetElement1Image(elementType);
        }
    }

    public void RemoveEnemyElementImg(string enemyName, ElementTypeCode elementType)
    {
        if (enemyHpUIShowsDict.ContainsKey(enemyName))
        {
            enemyHpUIShowsDict[enemyName].hpUI.GetComponent<EnemyHpUIItem>().RemoveElement1Image(elementType);
        }
    }

    public void PlayElementReactionName(ElementReactionCode reactionCode, Vector3 position)
    {
        ElementReactionNameShows elementReactionNameShows = GameObjectPools<ElementReactionNameShows>.GetObject();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        elementReactionNameShows.nameUI.transform.position = new Vector3(UnityEngine.Random.Range(screenPos.x - 100, screenPos.x + 100), UnityEngine.Random.Range(screenPos.y - 50, screenPos.y + 50), 0);
        elementReactionNameShows.nameUI.transform.SetParent(uiItemRoot);
        elementReactionNameShows.nameUI.SetActive(true);
        elementReactionNameShows.nameUI.GetComponent<ElementReactionNameUIItem>().PlayElementReactionName(reactionCode);
        elementReactionNameShows.endTime = Time.time + 1f;
        elementReactionShowsList.Add(elementReactionNameShows);
    }
    private void Update()
    {
        if (messageQueue.Count > 0 && isMessageShow == false)
        {
            lock (messageQueue)
            {
                string message = messageQueue.Dequeue();
                isMessageShow = true;
                SetMessage(message);
            }
        }
        CleanObjectPool();
    }

    private void CleanObjectPool()
    {
        for (int i = elementReactionShowsList.Count - 1; i >= 0; i--)
        {
            if (Time.time - elementReactionShowsList[i].endTime > 0)
            {
                elementReactionShowsList[i].nameUI.SetActive(false);
                GameObjectPools<ElementReactionNameShows>.RecyclePool(elementReactionShowsList[i]);
                elementReactionShowsList.RemoveAt(i);
            }
        }
    }

    private void SetMessage(string message)
    {
        SetActive(messageText);
        SetText(messageText, message);
        AnimationClip animationClip = messageShowAnimation.GetClip("MessageShowAni");
        messageShowAnimation.Play();
        //Close after a while
        StartCoroutine(AnimationPlayDone(animationClip.length, () =>
        {
            SetActive(messageText, false);
            isMessageShow = false;
        }));
    }

    private IEnumerator AnimationPlayDone(float second, Action aniPlayDoneCallBack)
    {
        yield return new WaitForSeconds(second);
        if (aniPlayDoneCallBack != null)
        {
            aniPlayDoneCallBack();
        }
    }

    private class EnemyHpUIShows
    {
        public GameObject hpUI = (GameObject)Instantiate(Resources.Load(EnemyConstant.EnemyHpUIItemPath));
    }

    private class ElementReactionNameShows
    {
        public GameObject nameUI = (GameObject)Instantiate(Resources.Load(EnemyConstant.ElementReactionNamePath));
        public float endTime;
    }

    private class AttackNumShows
    {
        public GameObject numUI = (GameObject)Instantiate(Resources.Load(EnemyConstant.AttackNumPath));
        public float endTime;
    }
}
