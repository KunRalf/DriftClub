using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Helpers
{
    public class MessagePrefab : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _button;

        public void Init(string title, string description, UnityAction callback = null)
        {
            _titleText.gameObject.SetActive(!string.IsNullOrEmpty(title));
            _titleText.text = title;
            _descriptionText.text = description;
            if(callback != null)
                _button.AddListener(callback);
            _button.AddListener(Destroy);
            
        }

        private void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}