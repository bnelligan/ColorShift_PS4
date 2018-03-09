using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_MovingPlatform : BasePath {
    
    // Prefab reference
    Platform _prefabPlat;
    // Path platform
    Platform _platform;

	// Use this for initialization
	void Awake () {
        // Set path range
		minDist = new Vector2(0,0);
        maxDist = new Vector2(20, 20);
        LoadPlatformPrefab();
	}
	
    // Load the correct platform prefab depending on the color
    private void LoadPlatformPrefab()
    {
        switch(_color)
        {
            case COLOR.BLUE:
                _prefabPlat = Resources.Load<Platform>("Prefabs/Platforms/Platform_Blue");
                break;
            case COLOR.RED:
                _prefabPlat = Resources.Load<Platform>("Prefabs/Platforms/Platform_Red");
                break;
            case COLOR.GREEN:
                _prefabPlat = Resources.Load<Platform>("Prefabs/Platforms/Platform_Green");
                break;
            default:
                _prefabPlat = Resources.Load<Platform>("Prefabs/Platforms/Platform_Default");
                break;
        }
    }

    public override bool CreatePath(Gate startGate, Gate endGate)
    {
        // Call and validate base implementation
        if (!base.CreatePath(startGate, endGate))
            return false;

        // Create platform
        GameObject clone = Instantiate(_prefabPlat.gameObject, transform);
        _platform = clone.GetComponent<Platform>();

        // Set platform movement pattern
        float period = _length; 
        _platform.SetMovementPattern(true, period, endGate.WorldPosition - startGate.WorldPosition);

        // Link the gates together
        Gate.ConnectGates(startGate, endGate, this);
        return true;
    }
}
