

namespace du.di {

    /// <summary> ゲームパッドのボタン </summary>
    public enum GPButton {
        Triangle, Circle, Cross, Square,
        RightShoulder1, LeftShoulder1, RightShoulder2, LeftShoulder2,
        RightStick, LeftStick,
        Select, Start,
    }

    /// <summary> ゲームパッドの方向入力の方向 </summary>
    public enum GPArrow {
        Left, Right, Up, Down, Any, Arrow_Max
    }
    public enum GPArrowDirection {
        Horizontal, Vertical
    }
    //public enum Trigger { LeftTrigger, RightTrigger }

    /// <summary> ゲームパッドの方向入力ソース </summary>
    public enum GPAxis { Any, LeftStick, RightStick, Dpad }

}