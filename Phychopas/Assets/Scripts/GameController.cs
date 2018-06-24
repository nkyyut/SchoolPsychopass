using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必ず追加するコンポーネント
[RequireComponent(typeof(CursorManager))]

public class GameController : MonoBehaviour {

    //操作の状態
    public enum ControlState
    {
        NormalControl,  //通常操作
        ItemControl,    //アイテムを操作中
        MobControl,     //モブを操作中
        MenuControl,    //メニューを操作中(現在使用する予定はない)
        ControlLess,    //操作不能
    }
    //アイテムイベント
    public enum ItemEvent
    {
        Sound,      //音がなる
    }

    public SceneChanger SceneMng;
    public MobBase[] ManagingMobs = new MobBase[256];
    public ItemBase[] ManagingItems = new ItemBase[256];

    private CursorManager CursorMng;    //カーソルコントロール用
    private ControlState ctrlState = ControlState.NormalControl;     //操作制御用
    private Psychopath ManagingPsychopath = null;  //管理中のサイコパス

    // Use this for initialization
    void Start()
    {
        CursorMng = this.GetComponent<CursorManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //概要 ： モブを管理に含める
    //引数 ： MobBase newMob  管理に追加する新しいモブ
    //返値 ： bool 成否    TRUE:成功/FALSE:失敗
    //詳細 ： GameControllerにモブを追加します。
    //        追加されたモブは「ItemEvent」を受け取ることができます。
    //        返値にFALSEが返った場合は既に管理に含まれるか
    //        管理の上限に達している場合です。
    public bool AddMob(MobBase newMob)
    {
        foreach (MobBase mobs in ManagingMobs)
        {   //管理中に同じモブがいないか確認する
            if (mobs == newMob)
            {   //既に管理されているのでエラー
                return false;
            }
        }
        for (int i = 0; i < ManagingMobs.Length; ++i)
        {   //管理に追加できそうなインデックスを探す
            if (ManagingMobs[i] == null)
            {   //追加処理
                ManagingMobs[i] = newMob;
                return true;
            }
        }
        //管理の追加に失敗したのでエラー
        return false;
    }

    //概要 ： アイテムを管理に含める
    //引数 ： IetmBase newItem  管理に追加する新しいアイテム
    //返値 ： bool 成否    TRUE:成功/FALSE:失敗
    //詳細 ： GameControllerにアイテムを追加します。
    //        追加されたアイテムは「ItemEvent」を受け取ることができます。
    //        返値にFALSEが返った場合は既に管理に含まれるか
    //        管理の上限に達している場合です。
    public bool AddItem(ItemBase newItem)
    {
        foreach (ItemBase mobs in ManagingItems)
        {   //管理中に同じモブがいないか確認する
            if (mobs == newItem)
            {   //既に管理されているのでエラー
                return false;
            }
        }
        for (int i = 0; i < ManagingMobs.Length; ++i)
        {   //管理に追加できそうなインデックスを探す

            if (ManagingMobs[i] == null)
            {   //追加処理
                ManagingItems[i] = newItem;
                return true;
            }
        }

        //管理の追加に失敗したのでエラー
        return false;
    }

    //概要 ： アイテムを管理に含める
    //引数 ： IetmBase newItem  管理に追加する新しいアイテム
    //返値 ： bool 成否    TRUE:成功/FALSE:失敗
    //詳細 ： GameControllerにアイテムを追加します。
    //        追加されたアイテムは「ItemEvent」を受け取ることができます。
    //        返値にFALSEが返った場合は既に管理に含まれるか
    //        管理の上限に達している場合です。
    public bool AddPsychopath(Psychopath newPsycho)
    {
        if (ManagingPsychopath)
        {   //err
            return false;
        }
        ManagingPsychopath = newPsycho;

        return true;
    }




    //概要 ： モブのキル判定(ひざ切り)
    //引数 ： Vectro2 clickPos    クリックされた座標
    //        MobBase killingMob  キル判定するモブ
    //返値 :　bool TRUE：キルされた/FALSE：失敗
    public bool KillCheckMob(Vector2 clickPos, MobBase killingMob)
    {
        //条件をクリアしたことにする
        foreach (MobBase mobs in ManagingMobs)
        {
            if (mobs == null)
            {   //err
                continue;
            }
            //モブのキル判定
            if(mobs.KillCheck(clickPos, killingMob, GetPsychoPos()))
            {   //殺すの失敗
                GameOver();
                return false;
            }
        }

        //選択したモブは死ぬ
        if (ManagingPsychopath) {
            ManagingPsychopath.MobKilling(clickPos, killingMob);
        }
        else
        {   //err
            Debug.Log("ERROR : GameControllerにPsychopathが設定されていない。");
        }
        return true;
    }

    //概要 : 生存しているモブの数を取得します。
    //引数 : なし
    //返値 : int 生きているモブの数
    public int GetAlivingMobs()
    {
        int aliveNum = 0;

        for (int i = 0; i < ManagingMobs.Length; ++i)
        {
            if (ManagingMobs[i] == null)
            {
                continue;
            }
            //生存判定
            if (ManagingMobs[i].IsAliving())
            {
                ++aliveNum;
            }
        }

        return aliveNum;
    }

    //概要 : アイテムイベントを管理中のオブジェクトに通知する。
    //引数 : ItemEvent ItemEvent   アイテムイベント
    //       ItemBase item   イベントの発生源
    //       Vector2 pos     イベントが発生した座標
    //返値 : なし
    //詳細 : アイテムイベントを全てのモブとアイテムに通知します。
    public void NotifyItemEvent(ItemEvent Event, ItemBase Item, Vector2 Pos)
    {
        for (int i = 0; i < ManagingMobs.Length; ++i)
        {
            if (ManagingMobs[i] == null)
            {   //err
                continue;
            }
            //アイテムイベントを渡す
            ManagingMobs[i].ReceiveItemEvent(Event, Item, Pos);
        }
        for (int i = 0; i < ManagingItems.Length; ++i)
        {
            if (ManagingItems[i] == null)
            {   //err
                continue;
            }
            ////アイテムイベントを渡す
            //items[i].ReceiveItemEvent(Event,Item,Pos);
        }
    }
    //概要 : アイテムの操作状態を受け取る
    //引数 : bool state   カーソルの設定
    //返値 : なし
    //詳細 : 今はカーソルを変更するだけ
    public void ReceiveItemState(bool state)
    {
        if (state)
        {
            CursorMng.ChangeCursor(CursorManager.CursorState.NormalCursor);
        }
        else
        {
            CursorMng.ChangeCursor(CursorManager.CursorState.LockOnCursor);
        }
    }
    //ステージクリアの処理
    public void StageClear()
    {
        //今はなにもしない
        Debug.Log("Stage Clear!");
        SceneMng.ChangeNextScene();
    }
    //ゲームオーバーの処理
    public void GameOver()
    {
        //今はなにもしない
        Debug.Log("Game Over...");
        SceneMng.ChangeScene("Title");
    }

    //概要 : 操作の状態を取得する
    //引数 : なし
    //返値 : ControlState   操作制御の状態
    public ControlState GetCtrlState()
    {
        return ctrlState;
    }
    //概要 : メニューを操作状態に変更する
    //引数 : なし
    //返値 : なし
    public void ChangeMenuControl()
    {
        ctrlState = ControlState.MenuControl;
    }
    //概要 : 操作状態をクリアする
    //引数 : なし
    //返値 : なし
    public void ClearControl()
    {
        ctrlState = ControlState.NormalControl;
    }
    //サイコパスの座標を取得する
    //詳細 : エラーが発生したら(0,0)座標を返す
    public Vector2 GetPsychoPos()
    {
        if(ManagingPsychopath)
        {   //err
            return new Vector2(0,0);
        }
        return ManagingPsychopath.GetPos();
    }
    public void StageClearCheck()
    {
        int aliveNum = GetAlivingMobs();

        Debug.Log("モブの残り" + aliveNum);

        if (aliveNum == 0)
        {
            StageClear();
        }
    }
}
