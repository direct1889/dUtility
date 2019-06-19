//Author: Richard Pieterse
//Date: 16 May 2013
//Email: Merrik44@live.com

using UnityEngine;
using UInput = UnityEngine.Input;
using System.Collections.Generic;
using System;

using Button = du.di.GPButton;
using Arrow = du.di.GPArrow;
using ArrowDirection = du.di.GPArrowDirection;
using Axis = du.di.GPAxis;
using GPRawID = du.di.Id.GamePadRaw;

namespace GamepadInput {

    /// <summary>
    /// ゲームパッドコントローラからの入力を実際に取得する
    /// </summary>
    public static class GamePadImpl {

        /// <summary> ボタンが押された瞬間 </summary>
        public static bool GetButtonDown(Button button, GPRawID rawID)
            => UInput.GetKeyDown(GetKeycode(button, rawID));
        /// <summary> ボタンが放された瞬間 </summary>
        public static bool GetButtonUp(Button button, GPRawID rawID)
            => UInput.GetKeyUp(GetKeycode(button, rawID));
        /// <summary> ボタンが押されている間 </summary>
        public static bool GetButton(Button button, GPRawID rawID)
            => UInput.GetKey(GetKeycode(button, rawID));

        /// <summary>
        /// returns a specified axis
        /// </summary>
        /// <param name="axis">One of the analogue sticks, or the dpad</param>
        /// <param name="rawID">The controller number</param>
        /// <param name="raw">if raw is false then the controlIndex will be returned with a deadspot</param>
        /// <returns></returns>

        public static Vector2 GetAxis(Axis axis, GPRawID rawID, bool raw = false) {

            IList<AxisName> axisNameList = new List<AxisName>();
            string xName = "", yName = "";


            switch (axis) {
                case Axis.Dpad:
                    xName = "DPad_XAxis_" + (int)rawID;
                    yName = "DPad_YAxis_" + (int)rawID;
                    axisNameList.Add(new AxisName(xName, yName));
                    break;
                case Axis.LeftStick:
                    xName = "L_XAxis_" + (int)rawID;
                    yName = "L_YAxis_" + (int)rawID;
                    axisNameList.Add(new AxisName(xName, yName));
                    break;
                case Axis.RightStick:
                    xName = "R_XAxis_" + (int)rawID;
                    yName = "R_YAxis_" + (int)rawID;
                    axisNameList.Add(new AxisName(xName, yName));
                    break;
                case Axis.Any:
                    xName = "DPad_XAxis_" + (int)rawID;
                    yName = "DPad_YAxis_" + (int)rawID;
                    axisNameList.Add(new AxisName(xName, yName));
                    xName = "L_XAxis_" + (int)rawID;
                    yName = "L_YAxis_" + (int)rawID;
                    axisNameList.Add(new AxisName(xName, yName));
                    xName = "R_XAxis_" + (int)rawID;
                    yName = "R_YAxis_" + (int)rawID;
                    axisNameList.Add(new AxisName(xName, yName));
                    break;
            }

            Vector2 axisXY = Vector3.zero;

            try {
                foreach (AxisName elem in axisNameList) {
                    float x, y;

                    if (raw == false) {
                        x = UInput.GetAxis(elem.xName);
                        y = UInput.GetAxis(elem.yName);
                    }
                    else {
                        x = UInput.GetAxisRaw(elem.xName);
                        y = UInput.GetAxisRaw(elem.yName);
                    }

                    Vector2 vec = new Vector2(x, y);
                    float mag = vec.magnitude;

                    if (mag > 0f) {
                        axisXY.x = x;
                        axisXY.y = y;
                    }
                }


            }
            catch (System.Exception e) {
                Debug.LogError(e);
                Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
            }
            return axisXY;
        }



        public static bool GetArrowKey(Arrow arrow, GPRawID controlIndex) {
            Vector2 axis = GetAxis(Axis.Any, controlIndex);
            switch (arrow) {
                case Arrow.Left : return axis.x <= -1.0f;
                case Arrow.Right: return axis.x >=  1.0f;
                case Arrow.Up   : return axis.y >=  1.0f;
                case Arrow.Down : return axis.y <= -1.0f;
                case Arrow.Any  : return axis.x <= -1.0f || axis.x >= 1.0f || axis.y >= 1.0f || axis.y <= -1.0f;
                default         : return false;
            }
        }


        private static bool[,] previousIsArrow = new bool[(int)GPRawID.Index_Max, (int)Arrow.Arrow_Max];

        public static bool GetArrowKeyDown(Arrow arrow, GPRawID controlIndex) {
            bool isArrow = GetArrowKey(arrow, controlIndex);

            bool returnValue;

            try {
                if (previousIsArrow[(int)controlIndex, (int)arrow] == false) {
                    //前フレームで押されていない時
                    returnValue = isArrow;
                }
                else {
                    //前フレームで押されていた時
                    returnValue = false;
                }
            }
            catch (IndexOutOfRangeException ex) {
                Debug.Log(ex);
                Debug.Log("Array Length:" + previousIsArrow.Length);
                Debug.Log("control Index:" + controlIndex);
                return false;
            }

            //前フレームの情報を更新
            previousIsArrow[(int)controlIndex, (int)arrow] = GetArrowKey(arrow, controlIndex);
            return returnValue;
        }


        public static float GetArrowAxisDown(ArrowDirection direction, GPRawID controlIndex) {
            switch (direction) {
                case ArrowDirection.Horizontal:
                    if (GetArrowKeyDown(Arrow.Left, controlIndex)) {
                        return -1f;
                    }
                    if (GetArrowKeyDown(Arrow.Right, controlIndex)) {
                        return 1f;
                    }
                    break;
                case ArrowDirection.Vertical:
                    if (GetArrowKeyDown(Arrow.Down, controlIndex)) {
                        return -1f;
                    }
                    if (GetArrowKeyDown(Arrow.Up, controlIndex)) {
                        return 1f;
                    }
                    break;
            }


            return 0f;
        }

        //public static float GetTrigger(Trigger trigger, Index controlIndex, bool raw = false)
        //{
        //    //
        //    string name = "";
        //    if (trigger == Trigger.LeftTrigger)
        //        name = "TriggersL_" + (int)controlIndex;
        //    else if (trigger == Trigger.RightTrigger)
        //        name = "TriggersR_" + (int)controlIndex;

        //    //
        //    float axis = 0;
        //    try
        //    {
        //        if (raw == false)
        //            axis = UInput.GetAxis(name);
        //        else
        //            axis = UInput.GetAxisRaw(name);
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.LogError(e);
        //        Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
        //    }
        //    return axis;
        //}


        private static KeyCode GetKeycode(Button button, GPRawID controlIndex) {
            switch (controlIndex) {
                case GPRawID._1P:
                    switch (button) {
                        case Button.Triangle      : return KeyCode.Joystick1Button0;
                        case Button.Circle        : return KeyCode.Joystick1Button1;
                        case Button.Cross         : return KeyCode.Joystick1Button2;
                        case Button.Square        : return KeyCode.Joystick1Button3;
                        case Button.LeftShoulder1 : return KeyCode.Joystick1Button4;
                        case Button.RightShoulder1: return KeyCode.Joystick1Button5;
                        case Button.LeftShoulder2 : return KeyCode.Joystick1Button6;
                        case Button.RightShoulder2: return KeyCode.Joystick1Button7;
                        case Button.Select        : return KeyCode.Joystick1Button8;
                        case Button.Start         : return KeyCode.Joystick1Button9;
                        case Button.LeftStick     : return KeyCode.Joystick1Button10;
                        case Button.RightStick    : return KeyCode.Joystick1Button11;
                    }
                    break;

                case GPRawID._2P:
                    switch (button) {
                        case Button.Triangle      : return KeyCode.Joystick2Button0;
                        case Button.Circle        : return KeyCode.Joystick2Button1;
                        case Button.Cross         : return KeyCode.Joystick2Button2;
                        case Button.Square        : return KeyCode.Joystick2Button3;
                        case Button.LeftShoulder1 : return KeyCode.Joystick2Button4;
                        case Button.RightShoulder1: return KeyCode.Joystick2Button5;
                        case Button.LeftShoulder2 : return KeyCode.Joystick2Button6;
                        case Button.RightShoulder2: return KeyCode.Joystick2Button7;
                        case Button.Select        : return KeyCode.Joystick2Button8;
                        case Button.Start         : return KeyCode.Joystick2Button9;
                        case Button.LeftStick     : return KeyCode.Joystick2Button10;
                        case Button.RightStick    : return KeyCode.Joystick2Button11;
                    }
                    break;

                case GPRawID._3P:
                    switch (button) {
                        case Button.Triangle      : return KeyCode.Joystick3Button0;
                        case Button.Circle        : return KeyCode.Joystick3Button1;
                        case Button.Cross         : return KeyCode.Joystick3Button2;
                        case Button.Square        : return KeyCode.Joystick3Button3;
                        case Button.LeftShoulder1 : return KeyCode.Joystick3Button4;
                        case Button.RightShoulder1: return KeyCode.Joystick3Button5;
                        case Button.LeftShoulder2 : return KeyCode.Joystick3Button6;
                        case Button.RightShoulder2: return KeyCode.Joystick3Button7;
                        case Button.Select        : return KeyCode.Joystick3Button8;
                        case Button.Start         : return KeyCode.Joystick3Button9;
                        case Button.LeftStick     : return KeyCode.Joystick3Button10;
                        case Button.RightStick    : return KeyCode.Joystick3Button11;
                    }
                    break;

                case GPRawID._4P:
                    switch (button) {
                        case Button.Triangle      : return KeyCode.Joystick4Button0;
                        case Button.Circle        : return KeyCode.Joystick4Button1;
                        case Button.Cross         : return KeyCode.Joystick4Button2;
                        case Button.Square        : return KeyCode.Joystick4Button3;
                        case Button.LeftShoulder1 : return KeyCode.Joystick4Button4;
                        case Button.RightShoulder1: return KeyCode.Joystick4Button5;
                        case Button.LeftShoulder2 : return KeyCode.Joystick4Button6;
                        case Button.RightShoulder2: return KeyCode.Joystick4Button7;
                        case Button.Select        : return KeyCode.Joystick4Button8;
                        case Button.Start         : return KeyCode.Joystick4Button9;
                        case Button.LeftStick     : return KeyCode.Joystick4Button10;
                        case Button.RightStick    : return KeyCode.Joystick4Button11;
                    }
                    break;

                case GPRawID.Any:
                    switch (button) {
                        case Button.Triangle      : return KeyCode.JoystickButton0;
                        case Button.Circle        : return KeyCode.JoystickButton1;
                        case Button.Cross         : return KeyCode.JoystickButton2;
                        case Button.Square        : return KeyCode.JoystickButton3;
                        case Button.LeftShoulder1 : return KeyCode.JoystickButton4;
                        case Button.RightShoulder1: return KeyCode.JoystickButton5;
                        case Button.LeftShoulder2 : return KeyCode.JoystickButton6;
                        case Button.RightShoulder2: return KeyCode.JoystickButton7;
                        case Button.Select        : return KeyCode.JoystickButton8;
                        case Button.Start         : return KeyCode.JoystickButton9;
                        case Button.LeftStick     : return KeyCode.JoystickButton10;
                        case Button.RightStick    : return KeyCode.JoystickButton11;
                    }
                    break;
            }
            return KeyCode.None;
        }


        public static GamepadState GetState(GPRawID controlIndex, bool raw = false) {
            GamepadState state = new GamepadState();

            state.One = GetButton(Button.Triangle, controlIndex);
            state.Two = GetButton(Button.Circle, controlIndex);
            state.Three = GetButton(Button.Cross, controlIndex);
            state.Four = GetButton(Button.Square, controlIndex);

            state.RightShoulder1 = GetButton(Button.RightShoulder1, controlIndex);
            state.LeftShoulder1 = GetButton(Button.LeftShoulder1, controlIndex);
            state.RightShoulder2 = GetButton(Button.RightShoulder2, controlIndex);
            state.LeftShoulder2 = GetButton(Button.LeftShoulder2, controlIndex);

            state.RightStick = GetButton(Button.RightStick, controlIndex);
            state.LeftStick = GetButton(Button.LeftStick, controlIndex);

            state.Start = GetButton(Button.Start, controlIndex);
            state.Select = GetButton(Button.Select, controlIndex);

            state.LeftStickAxis = GetAxis(Axis.LeftStick, controlIndex, raw);
            state.rightStickAxis = GetAxis(Axis.RightStick, controlIndex, raw);
            state.dPadAxis = GetAxis(Axis.Dpad, controlIndex, raw);

            state.Left = (state.dPadAxis.x < 0 || state.LeftStickAxis.x <= -1.0f);
            state.Right = (state.dPadAxis.x > 0 || state.LeftStickAxis.x >= 1.0f);
            state.Up = (state.dPadAxis.y > 0 || state.LeftStickAxis.y >= 1.0f);
            state.Down = (state.dPadAxis.y < 0 || state.LeftStickAxis.y <= -1.0f);

            //state.LeftTrigger = GetTrigger(Trigger.LeftTrigger, controlIndex, raw);
            //state.RightTrigger = GetTrigger(Trigger.RightTrigger, controlIndex, raw);

            return state;
        }

        public static int GetPlayerNum(Button button) {

            for (GPRawID rawId = GPRawID._1P; rawId < GPRawID.Index_Max; ++rawId) {
                if (GetButtonDown(button, GPRawID._1P)) {
                    return (int)rawId;
                }
            }
            return -1;

        }


    }

    public class AxisName {
        public string xName { get; private set; }
        public string yName { get; private set; }

        public AxisName(string xName, string yName) {
            this.xName = xName;
            this.yName = yName;
        }
    }



    public class GamepadState {
        public bool One = false;
        public bool Two = false;
        public bool Three = false;
        public bool Four = false;
        public bool Start = false;
        public bool Select = false;
        public bool Left = false;
        public bool Right = false;
        public bool Up = false;
        public bool Down = false;
        public bool LeftStick = false;
        public bool RightStick = false;
        public bool RightShoulder1 = false;
        public bool LeftShoulder1 = false;
        public bool RightShoulder2 = false;
        public bool LeftShoulder2 = false;

        public Vector2 LeftStickAxis = Vector2.zero;
        public Vector2 rightStickAxis = Vector2.zero;
        public Vector2 dPadAxis = Vector2.zero;

        //public float LeftTrigger = 0;
        //public float RightTrigger = 0;

    }

}