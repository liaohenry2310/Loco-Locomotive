// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputAsset_Player.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputAsset_Player : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputAsset_Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputAsset_Player"",
    ""maps"": [
        {
            ""name"": ""Input"",
            ""id"": ""c454e5d3-f93d-446a-bba1-21048340d56c"",
            ""actions"": [
                {
                    ""name"": ""Directional"",
                    ""type"": ""Value"",
                    ""id"": ""6cc00bd9-8d2b-46ad-a57c-1c79b4c7ed75"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Primary"",
                    ""type"": ""Button"",
                    ""id"": ""445d21e0-c89f-4967-a307-0712ba942eca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PrimaryHold"",
                    ""type"": ""Button"",
                    ""id"": ""4bcb20d1-0c9c-4b66-856c-df3c8fc033cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Secondary"",
                    ""type"": ""Button"",
                    ""id"": ""025f2fdd-1ec9-4db1-8df1-6538b8be16a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SecondaryHold"",
                    ""type"": ""Button"",
                    ""id"": ""fee6eb6e-0fe3-4b07-a664-ebadd64a924d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4fb3156c-fc0f-4f04-866d-6004eba5b612"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ac0f01a-bcb0-48cf-b351-c2eb76f04a50"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""840aac7b-6eb1-467c-91d1-0554b505d8c5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0100c763-f1f6-4abc-a76b-38e313e623a9"",
                    ""path"": ""<Keyboard>/slash"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3121a135-beeb-4bd1-8b96-a616a6966c17"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67ae5b85-f037-464d-8918-d5eef06b3d93"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""1d488174-1722-4265-bfd9-5ee318e5d9c7"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Directional"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5e56ec80-c234-4201-bb7c-802fb7e7ca63"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5585295e-096c-43f8-ab2c-1e6581c635ba"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c5e3ce69-f978-49a5-932f-c25602eab629"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bcd23ad4-2d31-4545-9f73-23ac3774ad88"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Directional Keys"",
                    ""id"": ""39966502-5ae3-4fa8-9fc0-eed9ed879e54"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Directional"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9f9ff2c7-468e-46b0-955c-7fa62dcc47b5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""37f96b29-72b4-4ba0-b944-6cd9659b74c0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c20e93c2-9435-4f7d-89fd-009c0f2f95e3"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""18db1db9-3567-48cb-a27e-d8466610641a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""fe5b3647-e9db-48c1-87ff-bd854758d8a2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Directional"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0896cfcc-f650-4eff-901b-0d9687f69b8c"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4ee0a94e-bce6-4e0d-b263-ca803ef8052a"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dcf38b78-ff09-4858-918f-5e342a0a5b12"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f01c5ff9-cc9c-4692-b173-52d47ed4ee4c"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Directional"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""522c3b0c-5874-4e9a-9752-889c28b87115"",
                    ""path"": ""<Keyboard>/slash"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""SecondaryHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e221aab-fedb-4c27-a75f-6caf2e4e843c"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""SecondaryHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e67f8599-4e1b-417a-bba0-04409a71408c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondaryHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77100e1a-2225-4609-b5c5-0a82688c9ef2"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardRight"",
                    ""action"": ""PrimaryHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15e3d859-5af6-402a-a559-434548cf7e70"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardLeft"",
                    ""action"": ""PrimaryHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcba66c4-0880-4c5b-bc9b-0c99d340696d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PrimaryHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardRight"",
            ""bindingGroup"": ""KeyboardRight"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""KeyboardLeft"",
            ""bindingGroup"": ""KeyboardLeft"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Input
        m_Input = asset.FindActionMap("Input", throwIfNotFound: true);
        m_Input_Directional = m_Input.FindAction("Directional", throwIfNotFound: true);
        m_Input_Primary = m_Input.FindAction("Primary", throwIfNotFound: true);
        m_Input_PrimaryHold = m_Input.FindAction("PrimaryHold", throwIfNotFound: true);
        m_Input_Secondary = m_Input.FindAction("Secondary", throwIfNotFound: true);
        m_Input_SecondaryHold = m_Input.FindAction("SecondaryHold", throwIfNotFound: true);
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

    // Input
    private readonly InputActionMap m_Input;
    private IInputActions m_InputActionsCallbackInterface;
    private readonly InputAction m_Input_Directional;
    private readonly InputAction m_Input_Primary;
    private readonly InputAction m_Input_PrimaryHold;
    private readonly InputAction m_Input_Secondary;
    private readonly InputAction m_Input_SecondaryHold;
    public struct InputActions
    {
        private @InputAsset_Player m_Wrapper;
        public InputActions(@InputAsset_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Directional => m_Wrapper.m_Input_Directional;
        public InputAction @Primary => m_Wrapper.m_Input_Primary;
        public InputAction @PrimaryHold => m_Wrapper.m_Input_PrimaryHold;
        public InputAction @Secondary => m_Wrapper.m_Input_Secondary;
        public InputAction @SecondaryHold => m_Wrapper.m_Input_SecondaryHold;
        public InputActionMap Get() { return m_Wrapper.m_Input; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InputActions set) { return set.Get(); }
        public void SetCallbacks(IInputActions instance)
        {
            if (m_Wrapper.m_InputActionsCallbackInterface != null)
            {
                @Directional.started -= m_Wrapper.m_InputActionsCallbackInterface.OnDirectional;
                @Directional.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnDirectional;
                @Directional.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnDirectional;
                @Primary.started -= m_Wrapper.m_InputActionsCallbackInterface.OnPrimary;
                @Primary.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnPrimary;
                @Primary.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnPrimary;
                @PrimaryHold.started -= m_Wrapper.m_InputActionsCallbackInterface.OnPrimaryHold;
                @PrimaryHold.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnPrimaryHold;
                @PrimaryHold.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnPrimaryHold;
                @Secondary.started -= m_Wrapper.m_InputActionsCallbackInterface.OnSecondary;
                @Secondary.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnSecondary;
                @Secondary.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnSecondary;
                @SecondaryHold.started -= m_Wrapper.m_InputActionsCallbackInterface.OnSecondaryHold;
                @SecondaryHold.performed -= m_Wrapper.m_InputActionsCallbackInterface.OnSecondaryHold;
                @SecondaryHold.canceled -= m_Wrapper.m_InputActionsCallbackInterface.OnSecondaryHold;
            }
            m_Wrapper.m_InputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Directional.started += instance.OnDirectional;
                @Directional.performed += instance.OnDirectional;
                @Directional.canceled += instance.OnDirectional;
                @Primary.started += instance.OnPrimary;
                @Primary.performed += instance.OnPrimary;
                @Primary.canceled += instance.OnPrimary;
                @PrimaryHold.started += instance.OnPrimaryHold;
                @PrimaryHold.performed += instance.OnPrimaryHold;
                @PrimaryHold.canceled += instance.OnPrimaryHold;
                @Secondary.started += instance.OnSecondary;
                @Secondary.performed += instance.OnSecondary;
                @Secondary.canceled += instance.OnSecondary;
                @SecondaryHold.started += instance.OnSecondaryHold;
                @SecondaryHold.performed += instance.OnSecondaryHold;
                @SecondaryHold.canceled += instance.OnSecondaryHold;
            }
        }
    }
    public InputActions @Input => new InputActions(this);
    private int m_KeyboardRightSchemeIndex = -1;
    public InputControlScheme KeyboardRightScheme
    {
        get
        {
            if (m_KeyboardRightSchemeIndex == -1) m_KeyboardRightSchemeIndex = asset.FindControlSchemeIndex("KeyboardRight");
            return asset.controlSchemes[m_KeyboardRightSchemeIndex];
        }
    }
    private int m_KeyboardLeftSchemeIndex = -1;
    public InputControlScheme KeyboardLeftScheme
    {
        get
        {
            if (m_KeyboardLeftSchemeIndex == -1) m_KeyboardLeftSchemeIndex = asset.FindControlSchemeIndex("KeyboardLeft");
            return asset.controlSchemes[m_KeyboardLeftSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IInputActions
    {
        void OnDirectional(InputAction.CallbackContext context);
        void OnPrimary(InputAction.CallbackContext context);
        void OnPrimaryHold(InputAction.CallbackContext context);
        void OnSecondary(InputAction.CallbackContext context);
        void OnSecondaryHold(InputAction.CallbackContext context);
    }
}
