// GENERATED AUTOMATICALLY FROM 'Assets/InGameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InGameControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public InGameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InGameControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""a88e797f-23a3-46f5-92da-27c58c1e5bb7"",
            ""actions"": [
                {
                    ""name"": ""Toggle Camera"",
                    ""type"": ""Button"",
                    ""id"": ""f42082c5-5e03-4f22-83a7-08b76464a697"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""c3148d2f-7b84-42cf-9778-677bf7976116"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Button"",
                    ""id"": ""8b123f31-5c2c-4e46-a578-fcf85a20a061"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4d8f743b-a9fd-45b0-8a13-509d2e640a76"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a342b69c-790a-43e1-a766-00aeeaf1a32e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.1,max=0.9)"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""521e2f20-2dd6-4988-9d95-6e302d115e7a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_ToggleCamera = m_Gameplay.FindAction("Toggle Camera", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_ToggleCamera;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Look;
    public struct GameplayActions
    {
        private InGameControls m_Wrapper;
        public GameplayActions(InGameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleCamera => m_Wrapper.m_Gameplay_ToggleCamera;
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                ToggleCamera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleCamera;
                ToggleCamera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleCamera;
                ToggleCamera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleCamera;
                Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                ToggleCamera.started += instance.OnToggleCamera;
                ToggleCamera.performed += instance.OnToggleCamera;
                ToggleCamera.canceled += instance.OnToggleCamera;
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnToggleCamera(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
