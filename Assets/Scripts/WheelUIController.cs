using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class WheelUIController : MonoBehaviour
{
    [SerializeField] 
    private Transform _parent;

    [SerializeField] 
    private Image _wheelImage;
    
    [SerializeField]
    private int _wheelItemCount;
    
    [SerializeField] private float innerDeadzone = 0.2f;
    
    private int _currentSelectedIndex = -1;
    private float _anglePerItem;
    private Image[] _slices;
    
    void Start()
    {
        _slices = new Image[_wheelItemCount];
        _anglePerItem = 360f / _wheelItemCount;
        
        for (int i = 0; i < _wheelItemCount; i++)
        {
            Image img = Instantiate(_wheelImage, _parent).GetComponent<Image>();
            float zRotation = 180f + (_anglePerItem / 2f) - (i * _anglePerItem);
            
            img.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            img.fillAmount = 1.0f / _wheelItemCount;
            img.color = Random.ColorHSV();
            _slices[i] = img;
        }
    }

    private void Update()
    {
        Vector2 input = GetInputVector();
        if (input.sqrMagnitude > innerDeadzone * innerDeadzone)
        {
            UpdateSelection(input);
        }
        else
        {
            ClearSelection();
        }
    }

    private void UpdateSelection(Vector2 input)
    {
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        float normalizedAngle = 90f - angle;

        if (normalizedAngle < 0) 
            normalizedAngle += 360f;
        else if (normalizedAngle >= 360f)
            normalizedAngle -= 360f;

        int index = Mathf.RoundToInt(normalizedAngle / _anglePerItem);
        
        if (index == _wheelItemCount) index = 0;

        if (_currentSelectedIndex != index)
        {
            ChangeSelection(index);
        }
    }

    private void ChangeSelection(int newIndex)
    {
        if (_currentSelectedIndex >= 0)
        {
            _slices[_currentSelectedIndex].rectTransform.localScale = Vector3.one; 
        }

        _currentSelectedIndex = newIndex;

        if (_currentSelectedIndex >= 0)
        {
            _slices[_currentSelectedIndex].rectTransform.localScale = new Vector3(1.15f, 1.15f, 1.15f); 
        }
    }

    private void ClearSelection()
    {
        if (_currentSelectedIndex >= 0)
        {
            _slices[_currentSelectedIndex].rectTransform.localScale = Vector3.one;
            _currentSelectedIndex = -1;
        }
    }

    private Vector2 GetInputVector()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        return (mousePos - screenCenter).normalized;
    }
}