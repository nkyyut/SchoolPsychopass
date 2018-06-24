﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class MobNormal : MobBase {

    [SerializeField] private float moveSpeed;        // モブの移動速度
    private Animator animator;                       // アニメーター
    private Vector2 min;                             // カメラの左下の座標
    private Vector2 max;                             // カメラの右上の座標
    private System.Random r;                         // ランダム用の変数
    private float width;                             // 画像の横幅
    private float height;                            // 画像の縦幅

    void Start() {
        // ランダムインスタンスを生成
        r = new System.Random();
        // 左下と右上の座標を取得
        min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        max = Camera.main.ViewportToWorldPoint(Vector2.one);
        // 画像の大きさを取得
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        // 初期ポジションを設定
        transform.position = new Vector3(transform.position.x, min.y + height / 2);
        // アニメーターを取得
        animator = GetComponent(typeof(Animator)) as Animator;
    }

    void Update() {
        if (changeDirTimer > changeDirTiming) {
            // キャラの方向を変える
            ChangeDirection();
            changeDirTimer = 0;
        }
        changeDirTimer += Time.deltaTime;
        Move();
    }

    /***
     * キャラクターの向きを変えるメソッド
     */
    void ChangeDirection() {
        int temp = r.Next(4);
        // 絶対に被らないようにする
        while ((int)nowDir == temp) { temp = r.Next(4); }
        // 求めたランダムからEnumへ
        nowDir = (Dir)Enum.ToObject(typeof(Dir), temp);
        // 向きを変更
        if ((int)nowDir == 3) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[3];
            // スケール値取り出し
            Vector3 scale = new Vector3(-1, 1, 1);
            // 代入し直す
            transform.localScale = scale;
        }
        else if ((int)nowDir == 2) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[(int)nowDir];
            // スケール値取り出し
            Vector3 scale = new Vector3(1, 1, 1);
            // 代入し直す
            transform.localScale = scale;
        }
        else if ((int)nowDir == 1) {
            Debug.Log(nowDir + "きた！！");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[(int)nowDir];
        }
        // 現在の画像の名前を出力
        Debug.Log(dirs[(int)nowDir].name);
    }

    /***
     * キャラクターを移動させる
     */
    void Move() {
        if (nowDir == Dir.Left) {
            animator.enabled = true;
            if (!animator.GetBool("MobWolkOn")) animator.SetBool("MobWolkOn", true);
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if (nowDir == Dir.Right) {
            animator.enabled = true;
            if (!animator.GetBool("MobWolkOn")) {
                animator.SetBool("MobWolkOn", true);
            }
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else {
            animator.enabled = true;
            animator.SetBool("MobWolkOn", false);
            animator.SetBool("MobWolkOff", true);
            if ((int)nowDir == 1) {
                animator.enabled = false;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[1];
            }
        }
    }


    void ReceiveItemEvent(GameController.ItemEvent Event, ItemBase Item, Vector2 Pos) {
        switch (Event) {
            case GameController.ItemEvent.Sound:
                
                break;
            default:
                break;
        }
    }
}