﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Security.Policy;

public class MobBase : MonoBehaviour {

    public enum Dir { Front = 0, Rear  = 1,
                      Left  = 2, Right = 3 };       // 前後左右の情報
    public Sprite[] dirs = new Sprite[3];           // 前後左右のスプライト画像
    public Dir nowDir = Dir.Front;                  // 現在向いている方向
    public float changeDirTiming;                   // 画像を変えるタイミング
    public float changeDirTimer;                    // 画像を変えるタイマー
    [SerializeField] private GameController GameCtrler = null;
    private bool alivingFlg = true;                         // このキャラが生存しているかどうか


    // Use this for initialization
    void Start() {
        if(GameCtrler)
        {   //自身を管理に含める
            GameCtrler.AddMob(this);
        }
    }


    /***
     * @param pos マウスのクリックされたポジション
     * @param mb  クリックされたモブキャラの親クラス
     * @return このキャラクターを殺しても良いか
     */
    public bool KillCheck(Vector2 pos, MobBase mb) {
        // 正面なら殺せないゲームオーバー
        if (nowDir == Dir.Front) return true;
        // 左を向いている　かつ　自分より左にいる時　殺せないゲームオーバー
        if (nowDir == Dir.Left  && this.transform.position.x > pos.x) return true;
        // 右を向いている　かつ　自分より右にいる時　殺せないゲームオーバー
        if (nowDir == Dir.Right && this.transform.position.x < pos.x) return true;
        // それ以外は殺せる！！
        return false;
    }

    /***
     * このキャラを殺す！！メソッド
     * とりあえずこのモブを消すだけの処理
     */
    public void Killed() {
        alivingFlg = false;
    }

    /***
     * アイテムを使われた際の振り向くメソッド
     * @param アイテムの位置
     */
    public void ForceChangeDirection(Vector2 pos) {
        // タイマーをリセット
        changeDirTimer = 0;
        // 自身のオブジェクトより左側にある時左にそれ以外は右を向かせる
        nowDir = (transform.position.x > pos.x) ? Dir.Left : Dir.Right;
        // 自身のオブジェクトの方角を変える
        this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[(int)nowDir];
    }

    /***
     * モブが生きていたらtrueを返す。
     * 逆に死んで(ひざが切られて)いたらfalseを返す。
     * (勝手に追加しました、さいとう)
     */
     public bool IsAliving() 
     {
        return alivingFlg;
    }

    public void ReceiveItemEvent(GameController.ItemEvent Event, ItemBase Item, Vector2 Pos) {
        switch (Event) {
            case GameController.ItemEvent.Sound:
                if (transform.position.x > Pos.x) nowDir = Dir.Left;
                else nowDir = Dir.Right;
                break;
            default:
                break;
        }
    }
    //クリックされた時のイベント
    public void ClickDown()
    {
        if(GameCtrler.GetCtrlState() == GameController.ControlState.NormalControl)
        {
            GameCtrler.KillCheckMob(this.transform.position,this);
        }
    }
}
