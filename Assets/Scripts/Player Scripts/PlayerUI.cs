using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    
    // Reference to player script
    Player player;
    [SerializeField]
    private Slider HpBar;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    // Use this for initialization
    void Start () {
        HpBar.maxValue = player.MaxHP;
        HpBar.value = player.MaxHP;
    }
	
	// Update is called once per frame
	void Update () {
        HpBar.maxValue = player.MaxHP;
        HpBar.value = player.CurrentHP;
    }
}
