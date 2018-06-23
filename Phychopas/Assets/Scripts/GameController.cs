using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public MobBase[] ManagingMobs = new MobBase[256];
    public ItemBase[] ManagingItems = new ItemBase[256];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
    //概要 ： モブのキル判定(ひざ切り)
    //引数 ： Vectro2 clickPos    クリックされた座標
    //        MobBase killingMob  キル判定するモブ
    //返値 :　bool TRUE：キルされた/FALSE：失敗
    public bool KillCheckMob(Vector2 clickPos, MobBase killingMob)
    {
        //条件をクリアしたことにする
        foreach(MobBase mobs in ManagingMobs)
        {
            if(mobs == null)
            {   //err
                continue;
            }
            ////モブのキル判定
            //mobs.KillCheck(clickPos, killingMob);
        }

        ////選択したモブは死ぬ
        //killingMob.Killed();

        int aliveNum = GetAlivingMobs();
        return true;
    }

    //概要 : 生存しているモブの数を取得します。
    //引数 : なし
    //返値 : int 生きているモブの数
    //メモ : 未完の関数
    public int GetAlivingMobs()
    {
        int aliveNum = 0;

        foreach(MobBase mobs in ManagingMobs)
        {
            if (mobs == null)
            {
                continue;
            }
            ////生存判定
            //if(mobs.IsAliving())
            //{
            //    ++aliveNum;
            //}
        }
        //デバッグ用
        aliveNum = 1;

        return aliveNum;
    }

    //概要 : アイテムイベントを管理中のオブジェクトに通知する。
    //引数 : int ItemEvent   イベント番号
    //       ItemBase item   イベントの発生源
    //       Vector2 pos     イベントが発生した座標
    //返値 : なし
    //詳細 : アイテムイベントを全てのモブとアイテムに通知します。
    public void NotifyItemEvent(int ItemEvent, ItemBase Item, Vector2 Pos)
    {
        foreach (MobBase mobs in ManagingMobs)
        {
            if (mobs == null)
            {   //err
                continue;
            }
            ////
            //mobs.ReceiveItemEvent(ItemEvent,Item,Pos);
        }
        foreach (ItemBase items in ManagingItems)
        {
            if (items == null)
            {   //err
                continue;
            }
            ////
            //items.ReceiveItemEvent(ItemEvent,Item,Pos);
        }
    }
}
