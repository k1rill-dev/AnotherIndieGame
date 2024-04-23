using System.Collections;
using System.Collections.Generic;
using PlayerControlScripts;
using TMPro;
using UnityEngine;

namespace GameLogicScripts
{
    public class ChangeNotesInfo: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private GameObject _player;
        private PlayerController _playerController;
        private void Start()
        {
            _playerController = _player.GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            changeInfo(_playerController.NoteCount);
        }

        private void changeInfo(int notesCount)
        {
            string notesInfo = string.Format("Количество записок:\n{0}/10", notesCount);
            _textField.SetText(notesInfo);
        }
    }

}
