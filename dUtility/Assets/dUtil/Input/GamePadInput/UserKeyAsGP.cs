using System.Collections.Generic;
using System.Linq;

namespace du.di {

    /// <summary> GamePadとキーボードの入力対応表 </summary>
    public interface IUserKeyAsGP {
        /// <value> 方向入力対応 </value>
        IArrowKeyAsGP Arrow { get; }
        /// <value> ボタン入力対応 </value>
        IButtonKeyAsGP Button { get; }
    }

    /// <summary> 単一ユーザ用入力対応表 </summary>
    public class UserKeyAsGP : IUserKeyAsGP {
        #region field
        public IArrowKeyAsGP Arrow { get; }
        public IButtonKeyAsGP Button { get; }
        #endregion

        #region ctor
        public UserKeyAsGP(IArrowKeyAsGP arrow, IButtonKeyAsGP button) {
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
    public class AnyUserKeyAsGP : IUserKeyAsGP {
        #region field
        public IArrowKeyAsGP Arrow { get; }
        public IButtonKeyAsGP Button { get; }
        #endregion

        #region ctor
        public AnyUserKeyAsGP(IEnumerable<IArrowKeyAsGP> arrows, IEnumerable<IButtonKeyAsGP> buttons) {
            Arrow  = new AnyArrowKeyAsGP (arrows);
            Button = new AnyButtonKeyAsGP(buttons);
        }
        public AnyUserKeyAsGP(IEnumerable<IUserKeyAsGP> userInputs) {
            Arrow  = new AnyArrowKeyAsGP (userInputs.Select(ui => ui.Arrow ));
            Button = new AnyButtonKeyAsGP(userInputs.Select(ui => ui.Button));
        }
        #endregion

        #region getter
        public override string ToString() => $"Arrow[{Arrow}], Button[{Button}]";
        #endregion
    }

}
