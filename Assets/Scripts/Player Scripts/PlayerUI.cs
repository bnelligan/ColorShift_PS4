using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    Text _txtHealth;
    // Reference to player script
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    // Use this for initialization
    void Start () {
        _txtHealth.text = player.CurrentHP.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        _txtHealth.text = player.CurrentHP.ToString();
    }
}
