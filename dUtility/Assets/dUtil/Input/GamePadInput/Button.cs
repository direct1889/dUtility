using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UInput = UnityEngine.Input;

namespace du.di {

    /// <summary> ボタン対応表 </summary>
    public interface IButtonKeyAsGP {
        /// <summary> ボタンが押されている間:true </summary>
        bool Get(GPButton button);
        /// <summary> ボタンが押された瞬間:true </summary>
        bool GetDown(GPButton button);
        /// <summary> ボタンが放された瞬間:true </summary>
        bool GetUp(GPButton button);

        /// <summary> ボタンが押されている間:1f else 0f </summary>
        float GetF(GPButton button);
        /// <summary> ボタンが押された瞬間:1f else 0f </summary>
        float GetDownF(GPButton button);
        /// <summary> ボタンが放された瞬間:1f else 0f </summary>
        float GetUpF(GPButton button);
    }

    /// <summary>
    /// 抽象ボタン対応表
    /// - 実装によって異なるのはGet/GetDown/GetUpのみ、それ以外を共通化
    /// </summary>
    public abstract class AbsButtonKeyAsGP : IButtonKeyAsGP {
        #region getter
        public abstract bool Get    (GPButton button);
        public abstract bool GetDown(GPButton button);
        public abstract bool GetUp  (GPButton button);

        public float GetF    (GPButton button) => System.Convert.ToSingle(Get    (button));
        public float GetDownF(GPButton button) => System.Convert.ToSingle(GetDown(button));
        public float GetUpF  (GPButton button) => System.Convert.ToSingle(GetUp  (button));
        #endregion
    }

    /// <summary> 単一ユーザ用ボタン対応表 </summary>
    public class ButtonKeyAsGP : AbsButtonKeyAsGP {
        #region field
        /// <summary> 対応表の実体 </summary>
        IDictionary<GPButton, KeyCode> m_keys;
        #endregion

        #region ctor
        /// <param name="map"> ボタンとキー入力の対応辞書 </param>
        public ButtonKeyAsGP(IDictionary<GPButton, KeyCode> map) { m_keys = map; }
        #endregion

        #region getter
        public override bool Get    (GPButton button) => m_keys.ContainsKey(button) && UInput.GetKey    (m_keys[button]);
        public override bool GetDown(GPButton button) => m_keys.ContainsKey(button) && UInput.GetKeyDown(m_keys[button]);
        public override bool GetUp  (GPButton button) => m_keys.ContainsKey(button) && UInput.GetKeyUp  (m_keys[button]);

        public override string ToString() {
            return $"Two:{m_keys[GPButton.Circle]}, Three:{m_keys[GPButton.Cross]}, START:{m_keys[GPButton.Start]}";
        }
        #endregion
    }


    /// <summary>
    /// Anyユーザ用ボタン対応表
    /// - ButtonInputを複数持ち、そのいずれかが発火していれば全体も発火
    /// </summary>
    public class AnyButtonKeyAsGP : AbsButtonKeyAsGP {
        #region field
        IEnumerable<IButtonKeyAsGP> Buttons { get; }
        #endregion

        #region ctor
        public AnyButtonKeyAsGP(IEnumerable<IButtonKeyAsGP> buttons) { Buttons = buttons; }
        #endregion

        #region getter
        public override bool Get    (GPButton button) => Buttons.Any(i => i.Get(button));
        public override bool GetDown(GPButton button) => Buttons.Any(i => i.GetDown(button));
        public override bool GetUp  (GPButton button) => Buttons.Any(i => i.GetUp(button));
        #endregion
    }

}
