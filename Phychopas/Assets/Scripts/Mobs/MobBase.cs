using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MobBase : MonoBehaviour {
    
    [SerializeField] private Sprite[] dirs = new Sprite[4]; // 前後左右のスプライト画像
    [SerializeField] private int nowDir;                    // 現在向いている方向
    private float changeDirTimer;                           // 画像を変えるタイマー

	// Use this for initialization
	void Start () {
        // 自身の画像の名前を出力
        foreach (Sprite s in dirs) {
            Debug.Log(s.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (changeDirTimer > 0.1) {
            ChangeDirection();
            changeDirTimer = 0;
        }
        changeDirTimer += Time.deltaTime;
	}

    // モブの向きを変えるメソッド
    void ChangeDirection() {
        nowDir++;
        if (nowDir > 3) nowDir = 0;
        // 画像をチェンジ
        this.gameObject.GetComponent<SpriteRenderer>().sprite = dirs[nowDir];
        // 現在の画像の名前を出力
        Debug.Log(dirs[nowDir].name);
    }

    // 移動させるメソッド
    void Move () {
        
    }
}
