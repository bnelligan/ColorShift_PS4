using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

    [SerializeField]
    string _nextSceneName = "";
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (_nextSceneName != "")
            {
                // Load specified next scene
                SceneManager.LoadScene(_nextSceneName);
            }
            else
            {
                // Load the next scene in the build order
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            }
            
        }
    }
}
