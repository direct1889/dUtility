using System.Collections.Generic;
using System.Linq;

namespace du.di {

    /// <summary> GamePadとキーボードの入力対応表 </summary>
    public interface IUserInput {
        /// <value> 方向入力対応 </value>
        IArrowInput Arrow { get; }
        /// <value> ボタン入力対応 </value>
        IButtonInput Button { get; }
    }

    /// <summary> 単一ユーザ用入力対応表 </summary>
    public class UserInput : IUserInput {
        #region field
        public IArrowInput Arrow { get; }
        public IButtonInput Button { get; }
        #endregion

        #region ctor
        public UserInput(IArrowInput arrow, IButtonInput button) {
            Arrow = arrow; Button = button;
        }
        #endregion

        #region getter
        public override string ToString() => $"Arrow[{Arrow}], Button[{Button}]";
        #endregion
    }

    /// <summary>
    /// Anyユーザ用入力対応表
    /// - UserInputを複数持ち、そのいずれかが発火していれば全体も発火
    /// </summary>
    public class AnyUserInput : IUserInput {
        #region field
        public IArrowInput Arrow { get; }
        public IButtonInput Button { get; }
        #endregion

        #region ctor
        public AnyUserInput(IEnumerable<IArrowInput> arrows, IEnumerable<IButtonInput> buttons) {
            Arrow  = new AnyArrowInput (arrows);
            Button = new AnyButtonInput(buttons);
        }
        public AnyUserInput(IEnumerable<IUserInput> userInputs) {
            Arrow  = new AnyArrowInput (userInputs.Select(ui => ui.Arrow ));
            Button = new AnyButtonInput(userInputs.Select(ui => ui.Button));
        }
        #endregion

        #region getter
        public override string ToString() => $"Arrow[{Arrow}], Button[{Button}]";
        #endregion
    }

}
