using UnityEngine;
using System.Linq;


using GamePadRawID = du.di.Id.GamePadRaw;
using GamepadInput;

using UInput = UnityEngine.Input;
using PlayerID = du.di.Id.Player;
using GamePadID = du.di.Id.GamePad;
using static du.di.Id.IdExtension;
using static du.Ex.ExVector;



namespace du.di {

    public static class InputManager {


        #region public

        public static bool DebugKeyDown(KeyCode code) { return UInput.GetKeyDown(code); }


        public static void Initialize() {
            KeyInput4GamePad.Initialize();
            Id.IdConverter.Initialize();
        }


        public static bool GetButtonDown(PlayerID    plID, GPButton button) { return GetButtonDown(plID.ToRawID(), button); }
        public static bool GetButtonDown(GamePadID    gpID, GPButton button) { return GetButtonDown(gpID.ToRawID(), button); }
        public static bool GetButton    (GamePadID    gpID, GPButton button) { return GetButton     (gpID.ToRawID(), button); }

        public static Vector2 GetArrowDPadVec2(GamePadID gpID) { return GetArrowDPadVec2(gpID.ToRawID()); }
        public static bool GetArrowDPad(GamePadID gpID, GPArrow arrow) { return GetArrowDPad(gpID.ToRawID(), arrow); }

        public static Vector2 GetLeftAxis    (PlayerID    plID) { return GetLeftAxis(plID.ToRawID()); }
        public static Vector2 GetLeftAxis    (GamePadID    gpID) { return GetLeftAxis(gpID.ToRawID()); }
        public static Vector3 GetLeftAxisXZ    (PlayerID    plID) { return GetLeftAxis(plID.ToRawID()).ToXyZ(0f); }
        public static Vector3 GetLeftAxisXZ    (GamePadID    gpID) { return GetLeftAxis(gpID.ToRawID()).ToXyZ(0f); }

        #endregion


        #region private

        private static bool GetButtonDown(GamePadRawID gpRawID, GPButton button) {
            return GamePad.GetButtonDown(button, gpRawID)
                || di.KeyInput4GamePad.User(gpRawID).Button.GetDown(button);
        }
        private static bool GetButtonDown(
            GamePadRawID gpRawID,
            params GPButton[] buttons) {
            return buttons.Any(i => di.KeyInput4GamePad.User(gpRawID).Button.GetDown(i));
        }

        private static bool GetButtonUp(GamePadRawID gpRawID, GPButton button) {
            return GamePad.GetButtonUp(button, gpRawID)
                || di.KeyInput4GamePad.User(gpRawID).Button.GetUp(button);
        }

        private static bool GetButton(GamePadRawID gpRawID, GPButton button) {
            return GamePad.GetButton(button, gpRawID)
                || di.KeyInput4GamePad.User(gpRawID).Button.Get(button);
        }

        private static float GetArrow(GamePadRawID gpRawID, GPArrow arrow) {
            return System.Convert.ToInt32(GamePad.GetArrowKey(arrow, gpRawID));
        }

        private static float GetArrowKeyAsFloat(GamePadRawID gpRawID, GPArrow arrow) {
            return System.Convert.ToInt32(GamePad.GetArrowKey(arrow, gpRawID));
        }

        private static Vector2 GetArrowDPadVec2(GamePadRawID gpRawID) {
            return Ex.ExVector.ElemProcessing(
                GamePad.GetAxis(GPAxis.Dpad, gpRawID)
                + di.KeyInput4GamePad.User(gpRawID).Arrow.GetVector(),
                (a) => Mathf.Clamp(a, -1f, 1f)
                );
        }

        private static bool GetArrowDPad(GamePadRawID gpRawID, GPArrow arrow) {
            Vector2 dpad = GamePad.GetAxis(GPAxis.Dpad, gpRawID);
            switch (arrow) {
                case GPArrow.Left    : return dpad.x <= -1.0f;
                case GPArrow.Right    : return dpad.x >=  1.0f;
                case GPArrow.Up        : return dpad.y >=  1.0f;
                case GPArrow.Down    : return dpad.y <= -1.0f;
                case GPArrow.Any    : return
                        dpad.x <= -1.0f || dpad.x >=  1.0f ||
                        dpad.y >=  1.0f || dpad.y <= -1.0f;
                default                : return false;
            }
        }

        private static Vector2 GetLeftAxis(GamePadRawID gpRawID) {

            Vector2 total
                = GamePad.GetAxis(GPAxis.LeftStick, gpRawID)        //! アナログスティック
                + GamePad.GetAxis(GPAxis.Dpad, gpRawID)                //! 十字ボタン
                + KeyInput4GamePad.User(gpRawID).Arrow.GetVector(); //! キーボード

            if (total.magnitude <= 1f) { return total; }
            else { return total / total.magnitude; }

        }

        #endregion


    }

}
