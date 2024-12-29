using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUi : MonoBehaviour
{

    [SerializeField] private RectTransform _canvasRectTransform;
    private VirtualMouseInput _virtualMouseInput;

    private void Awake()
    {
        _virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    private void Update()
    {
        transform.localScale = Vector3.one * (1f / _canvasRectTransform.localScale.x);
        transform.SetAsLastSibling();
    }
    private void LateUpdate()
    {
        Vector2 virtualMousePosition = _virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);
        InputState.Change(_virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }
}
