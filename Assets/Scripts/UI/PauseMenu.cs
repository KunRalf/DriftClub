using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private bool isPaused = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
          
                Time.timeScale = 0f;
               
                AudioListener.pause = true;
            }
            else
            {
                
                Time.timeScale = 1f;
              
                AudioListener.pause = false;
            }
        }
    }
}