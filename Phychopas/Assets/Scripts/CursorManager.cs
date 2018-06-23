using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    public enum CursorState
    {
        NormalCursor,   //普通のカーソル
        LockOnCursor,   //ロックオンのカーソル
    }

    public Texture2D NormalCursorTexture;
    public Texture2D LockOnCursorTexture;

    // Use this for initialization
    void Start ()
    {
        ChangeCursor(CursorState.NormalCursor);
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    //概要 : カーソルの見た目を変更する
    //引数 : CursorState state    カーソルの状態
    //返値 : なし
    public void ChangeCursor(CursorState state)
    {
        Texture2D cursorTex = null;
        switch(state)
        {
            default:
            case CursorState.NormalCursor:
                if (NormalCursorTexture)
                {
                    cursorTex = NormalCursorTexture;
                }
                break;
            case CursorState.LockOnCursor:
                if (LockOnCursorTexture)
                {
                    cursorTex = LockOnCursorTexture;
                }
                break;
        }
        if (cursorTex == null)
        {   //デフォルトのカーソルを使う
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
            return;
        }
        //カーソルに画像を使う
        Cursor.SetCursor(cursorTex, new Vector2(cursorTex.width / 2, cursorTex.height / 2), CursorMode.ForceSoftware);
    }
}
