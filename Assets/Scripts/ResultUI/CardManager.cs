﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {

    [SerializeField]
    GameObject Card;

    [SerializeField]
    GameObject CardListPlayer1;

    [SerializeField]
    GameObject CardListPlayer2;

    [SerializeField]
    AudioClip goodAudio;
    [SerializeField]
    AudioClip badAudio;

    // Use this for initialization
    void Start () {
        CardList.InitAllCards();

        if (BattleHistory.fleetHistorys.Count >= 2)
        {
            StartCoroutine(ShowCards());
        }
    }

    void GenerateCard (GameObject targetList, string text, Color textColor, Color backgroundColor)
    {
        var card = Instantiate(Card);
        card.transform.SetParent(targetList.transform);
        Vector3 vector = card.transform.localPosition;
        vector.z = 0;
        card.transform.localPosition = vector;
        card.transform.localScale = Vector3.one;

        card.GetComponentInChildren<Text>().text = text;
        card.GetComponentInChildren<Text>().color = textColor;
        card.GetComponent<Image>().color = backgroundColor;
    }

    IEnumerator ShowCards(){
        yield return new WaitForSeconds(2);
        yield return CheckAllCards(CardListPlayer1, BattleHistory.fleetHistorys[0]);
        yield return CheckAllCards(CardListPlayer2, BattleHistory.fleetHistorys[1]);
    }

    IEnumerator CheckAllCards(GameObject targetList, FleetHistory fleetHistory)
    {
        foreach (Card card in CardList.cardList)
        {
            if (card.IsSatisfyAllConditions(fleetHistory)){
                GenerateCard(targetList, card.text, card.textColor, card.backgroundColor);
                if(card.isGood){
                    SFXManager.instance.Play(goodAudio);
                }
                else{
                    SFXManager.instance.Play(badAudio);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

public static class CardList
{
    public static List<Card> cardList = new List<Card>();

    public static void InitAllCards()
    {
        cardList.Clear();
        Card card;

        /*
         * Guide about how to add new card rule (or conditions)
         * Card 클래스 생성자 인자 설명 :  
         * 첫번째 인자 : card의 이름입니다. (설명 등을 표시하는 용도)
         * 두번째 인자 : card 문구 (실제로 유저에게 보이는 문구)
         * 세번째 인자 : card 문구의 색상
         * 네번째 인자 : card 의 배경 색상
         * 다섯번째 인자 : 컨디션 List 객체 
         * (Card는 List내 모든 Condition들이 모두 만족해야하는.. 즉, && 연산으로 처리됩니다)
         * 
         * CardCondition 은 
         * 첫번째 인자 : 해당 컨디션의 조건이 되는 값
         * 두번째 인자 : 첫번째 인자 값을 현재 FleetHistory 에서 이상이여야하는지, 이하여야하는지 표시
         * 이상일 경우 true, 이하일 경우 false를 넣는다.

            card = new Card(
            "승리",
            "승리",
            Color.black,
            Color.yellow,
            new List<CardCondition>());
            card.conditions.Add(new CardCondition_HP(1, true));
            cardList.Add(card);

            card = new Card(
                    "절대자",
                    "당신은 혹시 신?",
                    Color.gray,
                    Color.white,
                    new List<CardCondition>());
            card.conditions.Add(new CardCondition_HP(card, 100, true));
            card.conditions.Add(new CardCondition_HitRate(card, 100, true));
            cardList.Add(card);
        
        */

        Color gold = new Color(255, 215, 0);

        // 순수 체력 관련
        card = new Card(
                "압도적인 승리",
                "압도적인 힘으로!",
                Color.black,
                gold,
                new List<CardCondition>());
        card.conditions.Add(new CardCondition_HP(90, true));
        cardList.Add(card);

        // 순수 명중률 관련
        card = new Card(
                "높은 명중률",
                "명사수",
                Color.black,
                gold,
                new List<CardCondition>());
        card.conditions.Add(new CardCondition_HitRate(70, true));
        cardList.Add(card);

        card = new Card(
                "매우 높은 명중률",
                "백발백중",
                Color.black,
                gold,
                new List<CardCondition>());
        card.conditions.Add(new CardCondition_HitRate(100, true));
        cardList.Add(card);

        card = new Card(
                "낮은 명중률",
                "어딜보고 쏘는 거야?",
                Color.black,
                Color.gray,
                new List<CardCondition>());
        card.conditions.Add(new CardCondition_HitRate(20, false));
        cardList.Add(card);

        card = new Card(
                "명중률 0",
                "손가락은 움직일 줄 아는거지?",
                Color.black,
                Color.gray,
                new List<CardCondition>());
        card.conditions.Add(new CardCondition_HitRate(0, false));
        cardList.Add(card);

        card = new Card(
                "신중한 그대",
                "신중한 그대",
                Color.black,
                Color.gray,
                new List<CardCondition>());
        card.conditions.Add(new CardCondition_HitRate(60, true));
        card.conditions.Add(new CardCondition_TotalUseEnerge(BattleHistory.maxTotalEnergy * 0.7f, false));
        card.conditions.Add(new CardCondition_TotalUseEnerge(BattleHistory.maxTotalEnergy * 0.4f, true));
        cardList.Add(card);
    }
}

