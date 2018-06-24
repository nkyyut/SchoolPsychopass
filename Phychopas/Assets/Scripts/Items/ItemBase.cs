using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    public AudioClip clip;

    [SerializeField] private GameObject target;     // アイテムを取得する時のゴール
    [SerializeField] private float moveTime;        // アイテムを取得する時の速度
    [SerializeField] private float spd = 10f;       // なげる球の速さ
    private bool throwFlg = false;                  // アイテムをなげるフラグ
    private int moveFlg = 0;                        // 取得するフラグ 0:なげられてない 1:飛んでる最中 2:飛び終わった
    private Vector2 startPos;                       // 自身の最初のposition
    private Vector2 targetPos;                      // 目標のposition
    private float timer;                            // 時間を計るタイマー
    private float lastPosX;                         // 消える直前のX座標
    private AudioSource audio;

    [SerializeField] private GameController GameCtrler = null;


    // アイテムをクリックした時！
    public void OnClick()
    {
        // 飛んでる最中の状態に変える
        moveFlg = 1;
    }

    // 初めの一回だけ呼ばれるよ！
    void Start()
    {
        startPos = new Vector2(this.transform.position.x, this.transform.position.y);
        targetPos = new Vector2(target.transform.position.x, target.transform.position.y);

        if (GameCtrler)
        {
            GameCtrler.AddItem(this);
        }
        audio = GameObject.Find("AudioController").GetComponent<AudioSource>();
        Debug.Log(audio);

    }

    // フレーム毎に呼ばれるよ！
    void Update()
    {
        // 1なので取得している最中
        if (moveFlg == 1)
        {
            // タイマーの値を増やす
            timer += Time.deltaTime;
            // targetまでちょっとずつアイテムを移動させる
            transform.position = Vector3.Slerp(startPos, targetPos, timer / moveTime);
            // アイテムが無事到着した時に投げられる様にする
            if (timer / moveTime > 1) moveFlg = 2;
        }

        // 投げられる状態にある　かつ　画面をタッチしたとき
        if (moveFlg == 2 && Input.GetMouseButtonDown(0))
        {
            // 二度目のクリックは受け付けない為に増やす
            moveFlg++;
            // マウスのクリック位置を取得
            Vector3 mousePositionVec3 = Input.mousePosition;
            // ３次元なのでZ座標を消すためにのZ座標の反対の向きをたして０にする
            mousePositionVec3.z = -Camera.main.transform.position.z;
            // マウスのクリック位置を2Dで使えるようにする
            Vector2 vec = Camera.main.ScreenToWorldPoint(mousePositionVec3);
            // クリック位置と自身のオブジェクトから角度を割り出してあげる
            float zRotation = Mathf.Atan2(vec.y - transform.position.y, vec.x - transform.position.x) * Mathf.Rad2Deg;
            // 自身のオブジェクトの傾きを求めた角度へ変更
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            // オブジェクトを移動できるようにする
            throwFlg = true;
        }

        // 投げてる状態！
        if (throwFlg)
        {
            // ひたすらにクリックされた方向へ移動
            transform.Translate(Vector2.right * spd * Time.deltaTime);
            // 画面から消えたらデストロイする
            if (!GetComponent<SpriteRenderer>().isVisible)
            {
                audio.PlayOneShot(this.clip);
                Debug.Log("Destory!!!");
                // 削除する前に消える直前のX座標を保存する
                lastPosX = transform.position.x;
                // 自身のオプジェクトを削除
                Destroy(this.gameObject);
            }
        }
    }
}

