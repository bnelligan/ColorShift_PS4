using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour {

    [SerializeField]
    COLOR _color;
    SpriteRenderer _renderer;

    Animator _animator;

    [SerializeField]
    float _cooldown = 0.5f; // Seconds

    float _lastPress;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _lastPress = Time.time - _cooldown;
    }

    // Use this for initialization
    void Start () {
	}

    // Function called when the button is activated
    private void OnButtonPressed()
    {
        _lastPress = Time.time;
        ColorManager.ToggleColor(_color);
        _animator.Play("Pressed");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerMotor pm = collision.GetComponent<PlayerMotor>();
            if(Time.time > _lastPress + _cooldown && !pm.Grounded)
            {
                OnButtonPressed();
            }
        }
    }
    

}
