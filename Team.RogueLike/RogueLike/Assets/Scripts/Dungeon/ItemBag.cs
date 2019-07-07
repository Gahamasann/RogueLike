using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    public List<int> getItemId;

    // Start is called before the first frame update
    void Start()
    {
        /*GameManagerのgetItemIdを使うことで
         レベルを跨いでも値を保持しておける*/
        getItemId = GameManager.instance.getItemId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemID(int id)
    {
        getItemId.Add(id);
    }
}
