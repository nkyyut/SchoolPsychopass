using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    [SerializeField] private GameController GameCtrler = null;

    // Use this for initialization
    void Start () {
		if(GameCtrler)
        {
            GameCtrler.AddItem(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
