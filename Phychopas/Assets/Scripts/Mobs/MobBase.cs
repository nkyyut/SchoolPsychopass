using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class MobBase : MonoBehaviour {

    [SerializeField] private GameController GameCtrler = null;
     private bool alivingFlg = true;                         // このキャラが生存しているかどうか
    private enum Dir { Front, Rear, Left, Right };          // 前後左右の情報
    [SerializeField] private Sprite[] dirs = new Sprite[4]; // 前後左右のスプライト画像
    [SerializeField] private Dir nowDir = Dir.Front;        // 現在向いている方向
    [SerializeField] private float changeDirTimer;          // 画像を変えるタイマー


    // Use this for initialization
    void Start() {
        if(GameCtrler)
        {   //自身を管理に含める
            GameCtrler.AddMob(this);
        }
        // 自身の画像の名前を出力
        foreach (Sprite s in dirs) {
            Debug.Log(s.name);
        }
    }

    // Update is called once per frame
    void Update() {
        if (changeDirTimer > 0.1) {
            ChangeDirection();
            changeDirTimer = 0;
        }
        changeDirTimer += Time.deltaTime;
    }

    /***
     * キャラクターの向きを変えるメソッド
     */
    void ChangeDirection() {
        // 方角の変数を更新
        nowDir = (int)nowDir < 3 ? ++nowDir : 0;
        // 画像をチェンジ
        this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[(int)nowDir];
        // 現在の画像の名前を出力
        Debug.Log(dirs[(int)nowDir].name);
    }

    /***
     * @param pos マウスのクリックされたポジション
     * @param mb  クリックされたモブキャラの親クラス
     * @return このキャラクターを殺しても良いか
     */
    public bool KillChack(Vector2 pos, MobBase mb) {
        // 正面なら即殺す
        if (nowDir == Dir.Front) return true;
        // 左を向いている　かつ　自分より左にいる時　殺す
        if (nowDir == Dir.Left  && this.transform.position.x > pos.x) return true;
        // 右を向いている　かつ　自分より右にいる時　殺す
        if (nowDir == Dir.Right && this.transform.position.x < pos.x) return true;
        // それ以外は殺さない！ == ゲームオーバー
        return false;
    }

    /***
     * このキャラを殺す！！メソッド
     * このモブを消すだけの処理
     */
    public void Killed() {
        alivingFlg = false;
        Destroy(this);
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
}
