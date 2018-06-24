using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class MobNormal : MobBase {

    [SerializeField] private float moveSpeed = 5f;   // モブの移動速度
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    private Animator animator;                       // アニメーター
    private Vector2 min;                             // カメラの左下の座標
    private Vector2 max;                             // カメラの右上の座標
    private System.Random r;                         // ランダム用の変数
    private float width;                             // 画像の横幅
    private float height;                            // 画像の縦幅
    private bool isMoveLeft;                         // 左に移動できるか
    private bool isMoveRight;                        // 右に移動できるか
    private bool deleteObjectFlg;
    private float alpha;
    private SpriteRenderer rend;


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
        // コライダーの位置をセット
        wallLeft.transform.position = new Vector3(min.x, 0, 0);
        wallRight.transform.position = new Vector3(max.x, 0, 0);
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log("当たり判定に入った！！");
        if (other.name == "Left") {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        } else if (other.name == "Right") {
            //transform.position = new Vector3(transform.position.x, transform.position.y);
            //isMoveRight = false;
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
    }

    void Update() {
        if (base.alivingFlg) {
            if (changeDirTimer > changeDirTiming) {
                // キャラの方向を変える
                ChangeDirection();
                changeDirTimer = 0;
            }
            changeDirTimer += Time.deltaTime;
            if (!base.isPushed) Move();
        } else if (deleteObjectFlg) {
            alpha = alpha + Time.deltaTime * 0.5f;
            rend.material.color = new Color(0f, 0f, 0f, alpha);
            if (alpha > 1) Destroy(this.gameObject);
        } else {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = base.deadImage;
            Invoke("delteObject", 3);
        }

    }

    void deleteObject() {
        deleteObjectFlg = true;
    }

    /***
     * キャラクターの向きを変えるメソッド
     */
    void ChangeDirection() {
        int temp = r.Next(4);
        // 絶対に被らないようにする
        while ((int)nowDir == temp) {
            temp = r.Next(4);
            if (!isMoveLeft && temp == 2) continue;
            if (!isMoveRight && temp == 3) continue;
        }
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
            //if (!isMoveRight) isMoveRight = true;
            if (!animator.GetBool("MobWolkOn")) animator.SetBool("MobWolkOn", true);
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if (nowDir == Dir.Right) {
            animator.enabled = true;
            //if (!isMoveLeft) isMoveLeft = true;
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



}