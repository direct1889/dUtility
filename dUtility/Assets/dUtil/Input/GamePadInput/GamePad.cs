using UnityEngine;
using System.Linq;

using GamePadRawID = du.di.Id.GamePadRaw;
using GamepadInput;

using UInput = UnityEngine.Input;
using PlayerID = du.di.Id.Player;
using GamePadID = du.di.Id.GamePad;
using static du.di.Id.ExId;
using static du.Ex.ExVector;

namespace du.di {

    /// <summary>
    /// ゲームパッドによる入力の管理
    /// - デバッグ用としてキーボードからの入力も受け付ける
    /// - ゲームパッドからとキーボードからの入力の違いを吸収する
    /// </summary>
    public static class GamePad {
        #region public
        /// <summary> 初期化処理 </summary>
        public static void Initialize() {
            KeyInput4GamePad.Initialize();
            Id.IdConverter.Initialize();
        }
        #endregion

        #region getter
        /// <summary>
        /// キーボードによる入力はあくまでデバッグ用とみなす
        /// </summary>
        public static bool DebugKeyDown(KeyCode code) => UInput.GetKeyDown(code);

        /// <summary> ボタンが押された瞬間:true </summary>
        public static bool GetButtonDown(PlayerID  plID, GPButton button) => GetButtonDown(plID.ToRawID(), button);
        /// <summary> ボタンが押された瞬間:true </summary>
        public static bool GetButtonDown(GamePadID gpID, GPButton button) => GetButtonDown(gpID.ToRawID(), button);
        /// <summary> ボタンが押されている間:true </summary>
        public static bool GetButton    (GamePadID gpID, GPButton button) => GetButton     (gpID.ToRawID(), button);

        /// <summary> 十字ボタンの入力をVector2で取得 </summary>
        public static Vector2 GetArrowDPadVec2(GamePadID gpID) => GetArrowDPadVec2(gpID.ToRawID());
        /// <summary> 十字ボタンの指定方向が押されているか </summary>
        public static bool GetArrowDPad(GamePadID gpID, GPArrow arrow) => GetArrowDPad(gpID.ToRawID(), arrow);

        /// <summary> 方向入力(左)の入力をVector2で取得 </summary>
        public static Vector2 GetLeftAxis(PlayerID  plID) => GetLeftAxis(plID.ToRawID());
        /// <summary> 方向入力(左)の入力をVector2で取得 </summary>
        public static Vector2 GetLeftAxis(GamePadID gpID) => GetLeftAxis(gpID.ToRawID());
        /// <summary>
        /// 方向入力(左)の入力をVector3で取得
        /// - X:Horizontal / Y:0 / Z:Vertical
        /// </summary>
        public static Vector3 GetLeftAxisXZ(PlayerID  plID) => GetLeftAxis(plID.ToRawID()).AddY(0f);
        /// <summary>
        /// 方向入力(左)の入力をVector3で取得
        /// - X:Horizontal / Y:0 / Z:Vertical
        /// </summary>
        public static Vector3 GetLeftAxisXZ(GamePadID gpID) => GetLeftAxis(gpID.ToRawID()).AddY(0f);
        #endregion

        #region private
        /// <summary> ボタンが押された瞬間 : true </summary>
        private static bool GetButtonDown(GamePadRawID gpRawID, GPButton button) {
            return GamepadInput.GamePadImpl.GetButtonDown(button, gpRawID)
                || di.KeyInput4GamePad.User(gpRawID).Button.GetDown(button);
        }
        /// <summary> いずれかのボタンが押された瞬間 : true </summary>
        private static bool GetButtonDown(GamePadRawID gpRawID, params GPButton[] buttons) {
            return buttons.Any(i => di.KeyInput4GamePad.User(gpRawID).Button.GetDown(i));
        }

        /// <summary> ボタンが放された瞬間 : true </summary>
        private static bool GetButtonUp(GamePadRawID gpRawID, GPButton button) {
            return GamepadInput.GamePadImpl.GetButtonUp(button, gpRawID)
                || di.KeyInput4GamePad.User(gpRawID).Button.GetUp(button);
        }

        /// <summary> ボタンが押されている間 : true </summary>
        private static bool GetButton(GamePadRawID gpRawID, GPButton button) {
            return GamepadInput.GamePadImpl.GetButton(button, gpRawID)
                || di.KeyInput4GamePad.User(gpRawID).Button.Get(button);
        }

        /// <summary> 十字ボタンの指定方向が押されているか </summary>
        /// <returns> 押されている:1f, 押されていない:0f </returns>
        private static float GetArrow(GamePadRawID gpRawID, GPArrow arrow) {
            // true -> 1f / false -> 0f
            return System.Convert.ToSingle(GamepadInput.GamePadImpl.GetArrowKey(arrow, gpRawID));
        }

        /// <summary> 十字ボタンの指定方向が押されているか </summary>
        /// <returns> 押されている:1f, 押されていない:0f </returns>
        private static float GetArrowKeyAsFloat(GamePadRawID gpRawID, GPArrow arrow) {
            return System.Convert.ToSingle(GamepadInput.GamePadImpl.GetArrowKey(arrow, gpRawID));
        }

        /// <summary> 十字ボタンの入力をVector2で取得 </summary>
        private static Vector2 GetArrowDPadVec2(GamePadRawID gpRawID) {
            return (GamepadInput.GamePadImpl.GetAxis(GPAxis.Dpad, gpRawID)
                + di.KeyInput4GamePad.User(gpRawID).Arrow.GetVector())
                .Clamped(-1f, 1f);
        }

        /// <summary> 十字ボタンの指定方向が押されているか </summary>
        private static bool GetArrowDPad(GamePadRawID gpRawID, GPArrow arrow) {
            Vector2 dpad = GamepadInput.GamePadImpl.GetAxis(GPAxis.Dpad, gpRawID);
            switch (arrow) {
                case GPArrow.Left  : return dpad.x <= -1.0f;
                case GPArrow.Right : return dpad.x >=  1.0f;
                case GPArrow.Up    : return dpad.y >=  1.0f;
                case GPArrow.Down  : return dpad.y <= -1.0f;
                case GPArrow.Any   : return dpad.x <= -1.0f || dpad.x >=  1.0f || dpad.y >=  1.0f || dpad.y <= -1.0f;
                default            : return false;
            }
        }

        /// <summary>
        /// 方向入力(左)をVector2で取得
        /// - 左スティック/十字ボタン/キーボード入力 を統合
        /// </summary>
        private static Vector2 GetLeftAxis(GamePadRawID gpRawID) {

            Vector2 total
                = GamepadInput.GamePadImpl.GetAxis(GPAxis.LeftStick, gpRawID)        // アナログスティック
                + GamepadInput.GamePadImpl.GetAxis(GPAxis.Dpad, gpRawID)             // 十字ボタン
                + KeyInput4GamePad.User(gpRawID).Arrow.GetVector(); // キーボード

            if (total.magnitude <= 1f) { return total; }
            else { return total / total.magnitude; }

        }

        #endregion


    }

}
