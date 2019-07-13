using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOnSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //自身を選択状態にする
        Selectable sel = GetComponent<Selectable>();
        sel.Select();
    }
}
