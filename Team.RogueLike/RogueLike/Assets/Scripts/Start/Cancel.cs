using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel : MonoBehaviour
{
    GameObject canvas;
    public GameObject player;
    KitiPlayer playerM;
    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.parent.gameObject;
        playerM = player.GetComponent<KitiPlayer>();
    }

    public void OnClick()
    {
        playerM.enabled = true;
        canvas.SetActive(false);
    }
}
