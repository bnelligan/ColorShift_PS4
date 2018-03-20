using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour {
    
    #region Private Variables
    [SerializeField]
    COLOR _color;
    [SerializeField]
    bool _active = false;
    SpriteRenderer _renderer;
    #endregion

    #region Access Variables
    public bool IsActive { get { return _active; } }
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        RegisterColorEvents();
        // Get component refs
        _renderer = GetComponent<SpriteRenderer>();

        // Set initial active state
        if (_active)
            Activate();
        else
            Inactivate();
    }
    public void OnDestroy()
    {
        UnregisterColorEvents();
    }
    #endregion

    #region Public Methods
    public void Activate()
    {
        _active = true;
        _renderer.enabled = true;

        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = true;
        }
    }
    public void Inactivate()
    {
        _active = false;
        _renderer.enabled = false;
        
        foreach(Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }
        // If this is a platform, remove player ref
        Platform plat = GetComponent<Platform>();
        if (plat)
        {
            plat.AttachedPlayer = null;
        }
    }
    #endregion

    #region Color Event Callbacks
    public void OnColorShift(COLOR color)
    {
        if(color == _color)
        {
            Activate();
        }
        else
        {
            Inactivate();
        }
    }
    public void OnColorActivate(COLOR color)
    {
        if(color == _color)
        {
            Activate();
        }
    }
    public void OnColorInactivate(COLOR color)
    {
        if(color == _color)
        {
            Inactivate();
        }

        
    }
    #endregion

    #region Private Methods
    private void RegisterColorEvents()
    {
        // Subscribe to color events
        ColorManager.OnActivateColor += OnColorActivate;
        ColorManager.OnInactivateColor += OnColorInactivate;
    }
    private void UnregisterColorEvents()
    {
        // Unubscribe from color events
        ColorManager.OnActivateColor -= OnColorActivate;
        ColorManager.OnInactivateColor -= OnColorInactivate;
    }
    #endregion

    
}
