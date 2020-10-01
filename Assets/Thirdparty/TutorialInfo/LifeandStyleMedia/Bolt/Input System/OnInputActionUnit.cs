﻿using Bolt;
using Lasm.OdinSerializer;
using Ludiq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lasm.BoltExtensions
{
    [TypeIcon(typeof(OnButtonInput))]
    [UnitTitle("On Input Action")]
    [UnitCategory("Events/Input/Input System")]
    public class OnInputActionUnit : ManualEventUnit<NullInputEventArgs>
    {
        protected override string hookName => "OnInputAction";

        [Inspectable][Tooltip("Runs the coroutine only once when first pressing, if the type is set to 'Pressing'.")]
        public bool cooldown;
        private bool cooldownStarted = false;

        [DoNotSerialize][PortLabelHidden]
        [NullMeansSelf]
        public ValueInput actions;

        [DoNotSerialize]
        public ValueOutput value;

        [DoNotSerialize]
        public ControlOutput performed;

        [UnitHeaderInspectable]
        [Inspectable]
        public InputActionAsset asset;

        [UnitHeaderInspectable]
        [Inspectable]
        public InputActionStatus status;

        [OdinSerialize]
        public InputAction action;

        [OdinSerialize]
        public InputActionMap map;

        private Action<InputAction.CallbackContext> act;

        public int actionId;

        private object lastValue;

        private GraphReference reference;

        private bool isPressing;

        private PlayerInput playerInput;

        private InputAction contextAction;

        protected override void Definition()
        {
            base.Definition();

            actions = ValueInput<PlayerInput>("actions", null).NullMeansSelf();

            if (action != null)
            {
                switch (action.expectedControlType)
                {
                    case "Vector2":
                        value = ValueOutput<Vector2>("value", (flow) => { return (Vector2)lastValue; });
                        break;

                    case "Vector3":
                        value = ValueOutput<Vector3>("value", (flow) => { return (Vector3)lastValue; });
                        break;

                    case "Integer":
                        value = ValueOutput<int>("value", (flow) => { return (int)lastValue; });
                        break;

                    case "Float":
                        value = ValueOutput<float>("value", (flow) => { return (float)lastValue; });
                        break;

                    case "Quaternion":
                        value = ValueOutput<Quaternion>("value", (flow) => { return (Quaternion)lastValue; });
                        break;

                    case "Euler":
                        value = ValueOutput<Vector3>("value", (flow) => { return (Vector3)lastValue; });
                        break;

                    default:
                        value = ValueOutput<object>("value", (flow) => { return lastValue; });
                        break;
                }
            }
        }

        public override void StartListening(GraphStack stack)
        {
            reference = stack.AsReference();
            var flow = Flow.New(reference);
            playerInput = flow.GetValue<PlayerInput>(actions);

            if (playerInput == null) playerInput = stack.self.GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                playerInput.actions[action.name].performed += Pressed; // Pressed
                playerInput.actions[action.name].canceled += Released; // Released
            }
        }

        private void Pressed(InputAction.CallbackContext context)
        {
            if (action != null)
            {
                contextAction = context.action;
                isPressing = true;

                if (status == InputActionStatus.Pressing)
                {
                    reference.component.StartCoroutine(Pressing());
                }
                else
                {
                    if (status == InputActionStatus.Pressed)
                    {
                        switch (action.expectedControlType)
                        {
                            case "Vector2":
                                lastValue = context.action.ReadValue<Vector2>();
                                break;

                            case "Vector3":
                                lastValue = context.action.ReadValue<Vector3>();
                                break;

                            case "Integer":
                                lastValue = context.action.ReadValue<int>();
                                break;

                            case "Float":
                                lastValue = context.action.ReadValue<float>();
                                break;

                            case "Quaternion":
                                lastValue = context.action.ReadValue<Quaternion>();
                                break;

                            case "Euler":
                                lastValue = context.action.ReadValue<Vector3>();
                                break;

                            default:
                                lastValue = context.action.ReadValueAsObject();
                                break;
                        }

                        Flow.New(reference).Invoke(trigger);
                    }
                }
            }
        }

        private IEnumerator Pressing()
        {
            while (isPressing)
            {
                switch (action.expectedControlType)
                {
                    case "Vector2":
                        lastValue = contextAction.ReadValue<Vector2>();
                        break;

                    case "Vector3":
                        lastValue = contextAction.ReadValue<Vector3>();
                        break;

                    case "Integer":
                        lastValue = contextAction.ReadValue<int>();
                        break;

                    case "Float":
                        lastValue = contextAction.ReadValue<float>();
                        break;

                    case "Quaternion":
                        lastValue = contextAction.ReadValue<Quaternion>();
                        break;

                    case "Euler":
                        lastValue = contextAction.ReadValue<Vector3>();
                        break;

                    default:
                        lastValue = contextAction.ReadValueAsObject();
                        break;
                }

                if (coroutine)
                {
                    if (cooldown && !cooldownStarted || !cooldown)
                    {
                        Flow.New(reference).StartCoroutine(trigger);
                        if (cooldown) cooldownStarted = true;
                    }
                }
                else
                {
                    Flow.New(reference).Invoke(trigger);
                }

                yield return null;
            }
        }

        private void Released(InputAction.CallbackContext context)
        {
            cooldownStarted = false;
            reference.component.StopCoroutine(Pressing());

            if (action != null)
            {
                isPressing = false;

                if (status == InputActionStatus.Released)
                {
                    switch (action.expectedControlType)
                    {
                        case "Vector2":
                            lastValue = context.action.ReadValue<Vector2>();
                            break;

                        case "Vector3":
                            lastValue = context.action.ReadValue<Vector3>();
                            break;

                        case "Integer":
                            lastValue = context.action.ReadValue<int>();
                            break;

                        case "Float":
                            lastValue = context.action.ReadValue<float>();
                            break;

                        case "Quaternion":
                            lastValue = context.action.ReadValue<Quaternion>();
                            break;

                        case "Euler":
                            lastValue = context.action.ReadValue<Vector3>();
                            break;

                        default:
                            lastValue = context.action.ReadValueAsObject();
                            break;
                    }

                    Flow.New(reference).Invoke(trigger);
                }
            }
        }

        public override void StopListening(GraphStack stack)
        {
            if (playerInput != null)
            {
                playerInput.actions[action.name].started -= Pressed;
                playerInput.actions[action.name].canceled -= Released;
            }
        }
    }

    public enum InputActionStatus
    {
        Pressed,
        Pressing,
        Released
    }
}