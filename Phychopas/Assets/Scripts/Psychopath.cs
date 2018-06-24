using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//必ず追加するコンポーネント
[RequireComponent(typeof(SpriteRenderer))]

public class Psychopath : MonoBehaviour {

    private enum Animation
    {
        AnimeBreak,     //アニメーションを中断する
        KillAnime,      //モブを殺すアニメーション
        TargetToMove,   //ターゲットに向かって移動
        TargetToMoveForMedic,//ばんそうこうを渡すために移動
        BackToDefalt,   //デフォルトの場所に移動する
        FirstPlayText,//最初のテキストの再生
        ClickWaitForText,//テキストのクリック待機状態
        GiveMedic,      //ばんそうこうを渡すアニメーション
    }
    private Vector2 defaultPos;
    private float animeTimer = 0;
    private int animeFrame;//コマ数
    private Animation animeState;
    private Vector2 targetPos;
    private SpriteRenderer mySprite;
    private bool defaultFlipX;
    private MobBase KillingMob = null;
    [SerializeField] private bool IsFirstTextPlay = false;
    [SerializeField] private SpriteRenderer FullScreenSprite = null;
    [SerializeField] private BoxCollider2D FullScreenCol = null;
    [SerializeField] private GameController GameCtrler;
    [SerializeField] private float MoveSpeed = 20.0f;
    [SerializeField] private float MoveSpeedForMedic = 5.0f;
    [SerializeField] private Sprite DefaultSprite = null;
    [SerializeField] private Sprite KillRunning = null;
    [SerializeField] private Sprite[] FirstTextAnime = new Sprite[8];
    [SerializeField] private Sprite[] WorkingAnime = new Sprite[1];
    [SerializeField] private Sprite[] KillingAnimes  = new Sprite[4];
    [SerializeField] private Sprite[] MedicAnimes  = new Sprite[4];
    // Use this for initialization
    void Start() {
        mySprite = this.GetComponent<SpriteRenderer>();

        defaultPos = this.transform.position;
        defaultFlipX = mySprite.flipX;
        if(GameCtrler)
        {
            GameCtrler.AddPsychopath(this);
        }
        if (IsFirstTextPlay)
        {
            animeState = Animation.FirstPlayText;
        }
        //MobKilling(new Vector2(0,0));
    }

    // Update is called once per frame
    void Update() {
        RunAnimation();
    }

    //アニメーションのコマを切り替える
    void RunAnimation()
    {
        int animeNum = 0;
        animeTimer += Time.deltaTime;
        mySprite.flipX = defaultFlipX;
        switch (animeState)
        {
            case Animation.AnimeBreak:
                animeTimer = 0.0f;
                animeState = Animation.AnimeBreak;
                animeFrame = 0;
                mySprite.sprite = DefaultSprite;
                mySprite.flipX = defaultFlipX;
                break;
            case Animation.KillAnime:
                if (animeTimer > 0.16f)
                {
                    FullScreenCol.enabled = true;
                    animeTimer = 0.0f;
                    ++animeFrame;
                    if (animeFrame > KillingAnimes.Length)
                    {   //次のアニメへ
                        this.transform.position = defaultPos;
                        animeFrame = 0;
                        animeState = Animation.TargetToMoveForMedic;
                        FullScreenSprite.sprite = null;
                        if (KillingMob)
                        {
                            KillingMob.Killed();
                        }
                    }
                    else
                    {   //コマ切り替え
                        FullScreenSprite.sprite = KillingAnimes[animeFrame - 1];
                    }
                }
                break;
            case Animation.TargetToMove:
                mySprite.sprite = KillRunning;
                this.transform.position = Vector2.Lerp(this.transform.position, targetPos, Time.deltaTime * MoveSpeed);

                if (Vector2.Distance(transform.position,targetPos) < 1.8f)
                {
                    animeTimer = 0.0f;
                    animeFrame = 0;
                    animeState = Animation.KillAnime;
                }
                break;
            case Animation.TargetToMoveForMedic:
                animeNum = (int)(animeTimer * 8) % (WorkingAnime.Length);
                animeNum = Mathf.Clamp(animeNum, 0, WorkingAnime.Length - 1);
                mySprite.sprite = WorkingAnime[animeNum];

                this.transform.position = Vector2.Lerp(this.transform.position, targetPos, Time.deltaTime * MoveSpeedForMedic);

                if (Vector2.Distance(transform.position, targetPos) < 2.5f)
                {
                    animeTimer = 0.0f;
                    animeFrame = 0;
                    animeState = Animation.GiveMedic;
                }
                break;
            case Animation.BackToDefalt:
                animeNum = (int)(animeTimer * 8) % (WorkingAnime.Length);
                animeNum = Mathf.Clamp(animeNum,0,WorkingAnime.Length - 1);
                mySprite.sprite = WorkingAnime[animeNum];
                mySprite.flipX = !defaultFlipX;

                this.transform.position = Vector2.Lerp(this.transform.position, defaultPos, Time.deltaTime * MoveSpeedForMedic);

                if (Vector2.Distance(transform.position, defaultPos) < 0.3f)
                {
                    animeTimer = 0.0f;
                    animeFrame = 0;
                    animeState = Animation.AnimeBreak;
                }
                break;
            case Animation.GiveMedic:
                if (animeTimer > 0.16f)
                {
                    animeTimer = 0.0f;
                    ++animeFrame;
                    if (animeFrame > MedicAnimes.Length)
                    {   //次のアニメへ
                        GameCtrler.ChangeMenuControl(); //メニュー操作状態にする
                        animeFrame = 0;
                        animeState = Animation.ClickWaitForText;
                    }
                    else
                    {   //コマ切り替え
                        FullScreenSprite.sprite = MedicAnimes[animeFrame - 1];
                    }
                }
                break;
            case Animation.FirstPlayText:
                if (animeTimer > 0.16f)
                {
                    FullScreenCol.enabled = true;
                    animeTimer = 0.0f;
                    ++animeFrame;
                    if (animeFrame > FirstTextAnime.Length)
                    {   //次のアニメへ
                        GameCtrler.ChangeMenuControl(); //メニュー操作状態にする
                        animeFrame = 0;
                        animeState = Animation.ClickWaitForText;
                    }
                    else
                    {   //コマ切り替え
                        FullScreenSprite.sprite = FirstTextAnime[animeFrame - 1];
                    }
                }
                break;
            case Animation.ClickWaitForText:
                animeTimer = 0.0f;
                animeFrame = 0;
                break;
            default:
                break;
        }
    }
    //モブを殺すアニメーションの再生を行う
    public void MobKilling(Vector2 killPos , MobBase killMob)
    {
        animeTimer = 0.0f;
        animeState = Animation.TargetToMove;
        targetPos = killPos;
        KillingMob = killMob;
    }
    //画面のクリックを受け取る
    public void ClearClickWait()
    {
        Debug.Log("Text Clear.");
        if(animeState == Animation.ClickWaitForText)
        {
            animeState = Animation.BackToDefalt;
            FullScreenSprite.sprite = null;
            FullScreenCol.enabled = false;
            GameCtrler.ClearControl();

            GameCtrler.StageClearCheck();
        }
    }
    //サイコパスの座標を取得する
    public Vector2 GetPos()
    {
        return this.transform.position;
    }
    
}
