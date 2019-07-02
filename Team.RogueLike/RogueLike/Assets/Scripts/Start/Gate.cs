using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject player;
    KitiPlayer playerM;
    GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        playerM = player.GetComponent<KitiPlayer>();
        canvas = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerM.enabled = false;
        canvas.SetActive(true);
    }
}
