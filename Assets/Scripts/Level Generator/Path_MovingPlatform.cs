using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_MovingPlatform : Path {
    
    // Prefab reference
    Platform _prefabPlat;
    // Path platform
    Platform _platform;

	// Use this for initialization
	void Awake () {
        // Set path range
		minDist = new Vector2(0,0);
        maxDist = new Vector2(20, 20);
        // Randomize the color
        int rcol = Random.Range(0, 4);
        switch(rcol)
        {
            case 0:
                _color = COLOR.BLUE;
                break;
            case 1:
                _color = COLOR.RED;
                break;
            case 2:
                _color = COLOR.GREEN;
                break;
            case 3:
                _color = COLOR.NONE;
                break;
        }
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
        GameObject clone = Instantiate(_prefabPlat.gameObject, startGate.transform.position, Quaternion.Euler(0,0,0));
        _platform = clone.GetComponent<Platform>();

        // Set platform movement pattern
        float period = _length/2; 
        _platform.SetMovementPattern(true, period, endGate.WorldPosition - startGate.WorldPosition);

        // Link the gates together
        Gate.ConnectGates(startGate, endGate, this);
        return true;
    }
}
