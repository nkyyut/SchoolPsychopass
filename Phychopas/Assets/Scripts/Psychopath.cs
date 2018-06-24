using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psychopath : MonoBehaviour {

    private enum Animation
    {
        AnimeBreak,     //アニメーションを中断する
        KillAnime,      //モブを殺すアニメーション
        TargetToMove,   //ターゲットに向かって移動
        BackToDefalt,   //デフォルトの場所に移動する
        GiveMedic,      //ばんそうこうを渡すアニメーション
    }
    private Transform defaultTrans;
    private float animeTimer = 0;
    private int animeFrame;//コマ数
    private Animation animeState;
    private Vector2 targetPos;
    [SerializeField] private GameController GameCtrler;
    [SerializeField] private float MoveSpeed = 20.0f;

    // Use this for initialization
    void Start() {
        defaultTrans = this.transform;
    }

    // Update is called once per frame
    void Update() {
        RunAnimation();
    }

    //アニメーションのコマを切り替える
    void RunAnimation()
    {
        animeTimer += Time.deltaTime;

        switch (animeState)
        {
            case Animation.AnimeBreak:
                animeTimer = 0.0f;
                animeState = Animation.BackToDefalt;
                animeFrame = 0;
                break;
            case Animation.KillAnime:
                switch (animeFrame)
                {
                    default:
                    case 1:
                        if(animeTimer > 0.16f)
                        {
                            animeTimer = 0;
                            ++animeFrame;
                        }
                        break;
                    case 2:
                        if (animeTimer > 0.16f)
                        {
                            animeTimer = 0;
                            ++animeFrame;
                        }
                        break;
                    case 3:
                        if (animeTimer > 0.16f)
                        {
                            animeTimer = 0;
                            ++animeFrame;
                        }
                        break;
                    case 4:
                        if (animeTimer > 0.16f)
                        {
                            animeTimer = 0;
                            animeFrame = 0;
                            animeState = Animation.GiveMedic;
                        }
                        break;
                }
                break;
            case Animation.TargetToMove:
                Vector2.Lerp(this.transform.position,targetPos, Time.deltaTime * MoveSpeed);
                break;
            case Animation.BackToDefalt:
                Vector2.Lerp(defaultTrans.position, targetPos, Time.deltaTime * MoveSpeed);
                break;
            case Animation.GiveMedic:
                animeTimer = 0;
                animeFrame = 0;
                animeState = Animation.BackToDefalt;
                break;
            default:
                break;
        }
    }
    //モブを殺すアニメーションの再生を行う
    void MobKilling(Vector2 killPos)
    {
        animeTimer = 0.0f;
        animeState = Animation.TargetToMove;
        targetPos = killPos;
    }

    
}
