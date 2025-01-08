using Helpers;
using Infrastructure.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Login
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_InputField _userNameInput;
        [SerializeField] private Button _button;
        
        private PlayerDataController _playerDataController;

        [Inject]
        public void Construct(PlayerDataController playerDataController)
        {
            _playerDataController = playerDataController;
            if (_playerDataController.Name == null)
            {
                _panel.SetActive(true);
                _button.AddListener(ConfirmUserName);
            }
            else
            {
                OpenGarageScene();
            }
        }

        private void ConfirmUserName()
        {
            _playerDataController.UpdateName(_userNameInput.text);
            OpenGarageScene();
        }

        private void OpenGarageScene()
        {
            SceneManager.LoadSceneAsync("Garage");
        }
    }
}