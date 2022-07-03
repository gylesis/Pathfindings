using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class AlgorithmSwitcher : MonoBehaviour
    {
        [SerializeField] private Toggle _astarToggle;
        [SerializeField] private Toggle _waveToggle;

        private AlgorithmType _algorithmType;

        public AlgorithmType AlgorithmType => _algorithmType;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _astarToggle.onValueChanged.AddListener((OnAstarToggle));
            _waveToggle.onValueChanged.AddListener((OnWaveToggle));
        }

        private void OnWaveToggle(bool state)
        {
            if (state)
                _algorithmType = AlgorithmType.Wave;
        }

        private void OnAstarToggle(bool state)
        {
            if (state)
                _algorithmType = AlgorithmType.Astar;
        }
        
    }
}