using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum COLOR { RED, BLUE, GREEN, NONE }
public class ColorManager : MonoBehaviour {
    
    #region Private Variables
    private Dictionary<COLOR, ColorNode> _colors;
    private ColorNode _activeColor;
    #endregion

    #region Public Variables
    public COLOR ActiveColor { get { return _activeColor.color; } }
    #endregion

    #region Color Events
    // Activate color event
    public delegate void evtActivateColor(COLOR color);
    public static event evtActivateColor OnActivateColor;
    // Inactivate color event
    public delegate void evtInactivateColor(COLOR color);
    public static event evtInactivateColor OnInactivateColor;
    // Toggle color event
    public delegate void evtToggleColor(COLOR color);
    public static event evtToggleColor OnToggleColor;

    public delegate void evtShiftRight();
    public static event evtShiftRight OnShiftRight;

    public delegate void evtShiftLeft();
    public static event evtShiftLeft OnShiftLeft;
    #endregion

    #region Internal Events
    // Events called by public static methods that echo to the ColorManager instance
    private delegate void evtResetLevel();
    private static event evtResetLevel OnResetLevel;
    #endregion


    #region Unity Callbacks
    private void Awake()
    {
        _colors = new Dictionary<COLOR, ColorNode>();
        InitializeColors();
    }
    private void OnEnable()
    {
        RegisterCallbacks();
    }
    private void OnDisable()
    {
        UnregisterCallbacks();
    }
    #endregion

    #region Private Methods
    private void InitializeColors()
    {
        // Setup list of colors
        _colors.Clear();
        // Create color nodes
        ColorNode redNode = new ColorNode(COLOR.RED);
        ColorNode blueNode = new ColorNode(COLOR.BLUE);
        ColorNode greenNode = new ColorNode(COLOR.GREEN);
        // Link nodes together to form a circular queue
        redNode.LeftNode = blueNode;
        redNode.RightNode = greenNode;
        blueNode.LeftNode = greenNode;
        blueNode.RightNode = redNode;
        greenNode.LeftNode = redNode;
        greenNode.RightNode = blueNode;
        // Add nodes to dictionary
        _colors[COLOR.RED] = redNode;
        _colors[COLOR.BLUE] = blueNode;
        _colors[COLOR.GREEN] = greenNode;


        ActivateColor(COLOR.GREEN);

    }
    #endregion

    #region Public Methods
    // Toggle a color
    public static void ToggleColor(COLOR color)
    {
        OnToggleColor(color);
    }
    // Activate a color
    public static void ActivateColor(COLOR color)
    {
        OnActivateColor(color);
    }
    // Inactivate a color
    public static void InactivateColor(COLOR color)
    {
        OnInactivateColor(color);
    }
    // Reset the level
    public static void ResetLevel()
    {
        OnResetLevel();
    }
    #endregion


    #region Private Event Callbacks
    /// <summary>
    /// Register instance callbacks
    /// </summary>
    private void RegisterCallbacks()
    {
        // Add listener to static events on class instance.
        OnActivateColor += ActivateColorCallback;
        OnInactivateColor += InactivateColorCallback;
        OnToggleColor += ToggleColorCallback;
        OnShiftLeft += ShiftLeftCallback;
        OnShiftRight += ShiftRightCallback;

        // Add listeners to internal callbacks
        OnResetLevel += ResetLevelCallback;
    }
    /// <summary>
    /// Unregister instance callbacks
    /// </summary>
    private void UnregisterCallbacks()
    {
        // Remove color callback event listeners
        OnActivateColor -= ActivateColorCallback;
        OnInactivateColor -= InactivateColorCallback;
        OnToggleColor -= ToggleColorCallback;
        OnShiftLeft -= ShiftLeftCallback;
        OnShiftRight -= ShiftRightCallback;

        // Remove listeners from internal callbacks
        OnResetLevel -= ResetLevelCallback;
    }
    // Callbacks for the manager instance
    private void ToggleColorCallback(COLOR color)
    {
        if(_colors[color].Active)
        {
            InactivateColor(color);
        }
        else
        {
            ActivateColor(color);
        }
    }
    private void ActivateColorCallback(COLOR color)
    {
        // Tag color as active
        _colors[color].Active = true;
    }
    private void InactivateColorCallback(COLOR color)
    {
        // Tag color as inactive
        _colors[color].Active = false;
    }
    private void ShiftRightCallback()
    {
        InactivateColor(_activeColor.color);
        _activeColor = _activeColor.RightNode;
        ActivateColor(_activeColor.color);
    }
    private void ShiftLeftCallback()
    {
        InactivateColor(_activeColor.color);
        _activeColor = _activeColor.LeftNode;
        ActivateColor(_activeColor.color);
    }
    private void ResetLevelCallback()
    {
    }
    #endregion

    public class ColorNode
    {
        public ColorNode LeftNode;
        public ColorNode RightNode;

        public COLOR color;
        public bool Active = false;

        public ColorNode(COLOR c)
        {
            color = c;
        }
    }
}

