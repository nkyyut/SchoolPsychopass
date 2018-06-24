using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class MobNormal : MobBase {
    
    private Vector2 min;            // カメラの左下の座標
    private Vector2 max;            // カメラの右上の座標
    private System.Random r;        // ランダム用の変数
    private bool moveLR;            // 左右に移動するための変数   0:Left 1:Right
    private float width;            // 画像の横幅
    private float height;           // 画像の縦幅
    private float moveSpeed;        // モブの移動速度

    void Start() {
        r = new System.Random();
        moveLR = r.Next(2) == 0 ? true : false;
        min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        max = Camera.main.ViewportToWorldPoint(Vector2.one);
        width  = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update() {
        if (changeDirTimer > changeDirTiming) {
            ChangeDirection();
            changeDirTimer = 0;
        }
        changeDirTimer += Time.deltaTime;
    }

    /***
     * キャラクターの向きを変えるメソッド
     */
    void ChangeDirection() {
        int temp = r.Next(4);
        // 絶対に被らないようにする
        while ( (int)nowDir == temp ) { temp = r.Next(4); }
        // 求めたランダムからEnumへ
        nowDir = (Dir)Enum.ToObject(typeof(Dir), temp);
        // 向きを変更
        this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[(int)nowDir];
        // 現在の画像の名前を出力
        Debug.Log(dirs[(int)nowDir].name);
    }

    /***
     * キャラクターを移動させる
     */
    void Move () {
        //if ( nowDir == Dir.Left ) {
            
        //}
    }
}
