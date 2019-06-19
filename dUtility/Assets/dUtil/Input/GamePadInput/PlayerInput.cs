using PlayerID = du.di.Id.Player;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace du.di {

    /// <summary> プレイヤー入力 </summary>
    public interface IPlayerInput {
        #region getter
        /// <summary> ボタンが押された瞬間:true </summary>
        bool GetButtonDown(GPButton button);
        /// <summary> ボタンが押されている間:true </summary>
        bool GetButton    (GPButton button);

        /// <summary> 十字ボタンの入力をVector2で取得 </summary>
        Vector2 GetArrowDPadVec2();
        /// <summary> 十字ボタンの指定方向が押されているか </summary>
        bool GetArrowDPad(GPArrow arrow);

        /// <summary> 方向入力(左)の入力をVector2で取得 </summary>
        Vector2 GetLeftAxis();
        /// <summary>
        /// 方向入力(左)の入力をVector3で取得
        /// - X:Horizontal / Y:0 / Z:Vertical
        /// </summary>
        Vector3 GetLeftAxisXZ();
        #endregion
    }

    /// <summary> プレイヤー入力 </summary>
    public class PlayerInput : IPlayerInput {
        #region field
        PlayerID PlayerID { get; }
        #endregion

        #region ctor
        public PlayerInput(PlayerID plID) => PlayerID = plID;
        #endregion

        #region getter
        /// <summary> ボタンが押された瞬間:true </summary>
        public bool GetButtonDown(GPButton button)
            => GamePad.GetButtonDown(PlayerID, button);
        /// <summary> ボタンが押されている間:true </summary>
        public bool GetButton(GPButton button)
            => GamePad.GetButton(PlayerID, button);

        /// <summary> 十字ボタンの入力をVector2で取得 </summary>
        public Vector2 GetArrowDPadVec2()
            => GamePad.GetArrowDPadVec2(PlayerID);
        /// <summary> 十字ボタンの指定方向が押されているか </summary>
        public bool GetArrowDPad(GPArrow arrow)
            => GamePad.GetArrowDPad(PlayerID, arrow);

        /// <summary> 方向入力(左)の入力をVector2で取得 </summary>
        public Vector2 GetLeftAxis()
            => GamePad.GetLeftAxis(PlayerID);
        /// <summary>
        /// 方向入力(左)の入力をVector3で取得
        /// - X:Horizontal / Y:0 / Z:Vertical
        /// </summary>
        public Vector3 GetLeftAxisXZ()
            => GamePad.GetLeftAxisXZ(PlayerID);
        #endregion
    }

}
