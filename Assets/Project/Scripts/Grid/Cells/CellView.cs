using TMPro;
using UnityEngine;

namespace Project.Grid.Cells
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TMP_Text _text;

        private Color _defaultColor;

        private void Awake()
        {
            _defaultColor = _sprite.color;
        }

        public void Highlight(Color color)
        {
            _sprite.color = color;
        }

        public void HighlightDefault()
        {
            Highlight(_defaultColor);
        }
        
        public void SetText(int value)
        {
            if (value == -1)
                _text.text = "";
            else
                _text.text = value.ToString();
        }
    }
}