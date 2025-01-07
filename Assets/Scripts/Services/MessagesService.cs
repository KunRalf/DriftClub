using System;
using Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class MessagesService : MonoBehaviour
    {
        [Header("With ahree button")]
        [SerializeField] private MessagePrefab _prefab;
        [SerializeField] private Transform _spawnPlace;
        
        
        public void InitMessage(string title, string description, UnityAction callback = null)
        {
            var message = Instantiate(_prefab, _spawnPlace);
            message.Init(title, description,callback);
        }

       
    }
}