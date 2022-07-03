using TMPro;
using UnityEngine;
using Zenject;

namespace Project.UI
{
    public class DrawToolsSwitcher : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        private int _currentTool;

        public int CurrentTool => _currentTool;

        [Inject]
        private void Init()
        {
            _dropdown.onValueChanged.AddListener((OnDropDownChanged));
        }

        private void OnDropDownChanged(int value)
        {
            _currentTool = value;
        }
    }
}