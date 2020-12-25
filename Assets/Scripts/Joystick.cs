using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private float handleRange = 1;

    [SerializeField]
    private float deadZone = 0;

    [SerializeField]
    private JoystickAxis joystickAxis = JoystickAxis.Both;

    [SerializeField]
    private bool snapX = false;

    [SerializeField]
    private bool snapY = false;

    [SerializeField]
    private bool autoHide = false;

    [SerializeField]
    private RectTransform background = null;

    [SerializeField]
    private RectTransform handle = null;

    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera uiCamera;

    private Vector2 input = Vector2.zero;

    public float Horizontal => (snapX) ? SnapToAxis(input.x, JoystickAxis.Horizontal) : input.x;

    public float Vertical => (snapY) ? SnapToAxis(input.y, JoystickAxis.Vertical) : input.y;

    public Vector2 Direction => new Vector2(Horizontal, Vertical);

    public float HandleRange
    {
        get => handleRange;
        set => handleRange = Mathf.Abs(value);
    }

    public float DeadZone
    {
        get => deadZone;
        set => deadZone = Mathf.Abs(value);
    }

    public JoystickAxis JoystickAxis
    {
        get => joystickAxis;
        set => joystickAxis = value;
    }

    public bool SnapX
    {
        get => snapX;
        set => snapX = value;
    }

    public bool SnapY
    {
        get => snapY;
        set => snapY = value;
    }

#region Monobehavior events

    private void Start()
    {
        if (autoHide)
        {
            background.gameObject.SetActive(false);
        }

        HandleRange = handleRange;
        DeadZone = deadZone;
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("The joystick must be a child of the canvas");
        }

        var center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }

#endregion

#region Pointer Handlers

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 newPos = ScreenPointToAnchoredPosition(eventData.position);

        if (!RectTransformUtility.RectangleContainsScreenPoint(background, eventData.position, uiCamera))
        {
            background.anchoredPosition = newPos;
        }

        background.gameObject.SetActive(true);
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (autoHide)
        {
            background.gameObject.SetActive(false);
        }

        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        uiCamera = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            uiCamera = canvas.worldCamera;
        }

        Vector2 position = RectTransformUtility.WorldToScreenPoint(uiCamera, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        InputToAxis();
        InputToDeadZone(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius * handleRange;
    }

#endregion

    private void InputToDeadZone(float magnitude, Vector2 normalised)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
            {
                input = normalised;
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }

    private void InputToAxis()
    {
        switch (joystickAxis)
        {
            case JoystickAxis.Horizontal:
                input = new Vector2(input.x, 0f);
                break;
            case JoystickAxis.Vertical:
                input = new Vector2(0f, input.y);
                break;
        }
    }

    private float SnapToAxis(float value, JoystickAxis snapAxis)
    {
        if (value == 0)
        {
            return value;
        }

        if (joystickAxis == JoystickAxis.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            switch (snapAxis)
            {
                case JoystickAxis.Horizontal when angle < 22.5f || angle > 157.5f:
                    return 0;
                case JoystickAxis.Horizontal:
                    return (value > 0) ? 1 : -1;
                case JoystickAxis.Vertical when angle > 67.5f && angle < 112.5f:
                    return 0;
                case JoystickAxis.Vertical:
                    return (value > 0) ? 1 : -1;
                default:
                    return value;
            }
        }

        if (value > 0)
        {
            return 1;
        }

        if (value < 0)
        {
            return -1;
        }

        return 0;
    }

    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, uiCamera,
            out Vector2 localPoint))
        {
            return Vector2.zero;
        }

        Vector2 sizeDelta;
        Vector2 pivotOffset = baseRect.pivot * (sizeDelta = baseRect.sizeDelta);
        return localPoint - (background.anchorMax * sizeDelta) + pivotOffset;
    }
}