using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class MobNormal : MobBase {

    [SerializeField] private float moveSpeed = 5f;   // モブの移動速度
    private Animator animator;                       // アニメーター
    private System.Random r;                         // ランダム用の変数
    private float width;                             // 画像の横幅
    private float height;                            // 画像の縦幅
    private bool deleteObjectFlg;
    private float alpha;
    private SpriteRenderer rend;
    public Sprite deadImage;
    private Color color;
    private Vector2 min;                             // カメラの左下の座標
    private Vector2 max;                             // カメラの右上の座標


    void Start() {
        // ランダムインスタンスを生成
        r = new System.Random();
        // 画像の大きさを取得
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        // 初期ポジションを設定
        min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        max = Camera.main.ViewportToWorldPoint(Vector2.one);
        transform.position = new Vector3(transform.position.x, min.y + height / 2);
        // アニメーターを取得
        animator = GetComponent(typeof(Animator)) as Animator;
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
            if (rend.material.color.a > 0) {
                alpha = rend.material.color.a - 0.5f * Time.deltaTime;
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, alpha);
            } else Destroy(this.gameObject);
        } else {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = deadImage;
            color = this.gameObject.GetComponent<SpriteRenderer>().material.color;
            Invoke("deleteObject", 3);
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
            if (transform.position.x < min.x) transform.Translate(Vector2.right * moveSpeed * 1.5f * Time.deltaTime);
        }
        else if (nowDir == Dir.Right) {
            animator.enabled = true;
            if (!animator.GetBool("MobWolkOn")) animator.SetBool("MobWolkOn", true);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x > max.x) transform.Translate(Vector2.left * moveSpeed * 1.5f * Time.deltaTime);
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