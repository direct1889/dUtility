using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UInput = UnityEngine.Input;

namespace du.di {

    /// <summary> 方向入力対応表 </summary>
    public interface IArrowInput {
        /// <summary> 方向入力が押されている間:true </summary>
        bool Get(GPArrow arrow);
        /// <summary> ボタンが押された瞬間:true </summary>
        bool GetDown(GPArrow arrow);
        /// <summary> ボタンが放された瞬間:true </summary>
        bool GetUp(GPArrow arrow);

        /// <summary> ボタンが押されている間:1f else 0f </summary>
        float GetF(GPArrow arrow);
        /// <summary> ボタンが押された瞬間:1f else 0f </summary>
        float GetDownF(GPArrow arrow);
        /// <summary> ボタンが放された瞬間:1f else 0f </summary>
        float GetUpF(GPArrow arrow);

        /// <summary> x,y ∈ [-1f, 1f] (右上が正) </summary>
        Vector2 GetVector();
        /// <summary> x,y ∈ [-1f, 1f] (右上が正) </summary>
        Vector2 GetVectorDown();
        /// <summary> x,y ∈ [-1f, 1f] (右上が正) </summary>
        Vector2 GetVectorUp();
    }

    /// <summary>
    /// 抽象方向入力対応表
    /// - 実装によって異なるのはGet/GetDown/GetUpのみ、それ以外を共通化
    /// </summary>
    public abstract class AbsArrowInput : IArrowInput {
        #region getter
        public abstract bool Get    (GPArrow arrow);
        public abstract bool GetDown(GPArrow arrow);
        public abstract bool GetUp  (GPArrow arrow);

        public float GetF    (GPArrow arrow) => System.Convert.ToSingle(Get(arrow));
        public float GetDownF(GPArrow arrow) => System.Convert.ToSingle(GetDown(arrow));
        public float GetUpF  (GPArrow arrow) => System.Convert.ToSingle(GetUp(arrow));

        public Vector2 GetVector() {
            return new Vector2(
                GetF(GPArrow.Right) - GetF(GPArrow.Left),
                GetF(GPArrow.Up   ) - GetF(GPArrow.Down)
                );
        }
        public Vector2 GetVectorDown() {
            return new Vector2(
                GetDownF(GPArrow.Right) - GetDownF(GPArrow.Left ),
                GetDownF(GPArrow.Down ) - GetDownF(GPArrow.Up   )
                );
        }
        public Vector2 GetVectorUp() {
            return new Vector2(
                GetUpF(GPArrow.Right) - GetUpF(GPArrow.Left ),
                GetUpF(GPArrow.Down ) - GetUpF(GPArrow.Up   )
                );
        }
        #endregion
    }

    /// <summary> 単一ユーザ用方向入力対応表 </summary>
    public class ArrowInput : AbsArrowInput {
        #region field
        /// <value> 対応表の実体 </value>
        IDictionary<GPArrow, KeyCode> Corresponds { get; }
        #endregion

        #region ctor
        /// <param name="up"> 方向入力上と対応するキー </param>
        /// <param name="down"> 方向入力下と対応するキー </param>
        /// <param name="left"> 方向入力左と対応するキー </param>
        /// <param name="right"> 方向入力右と対応するキー </param>
        public ArrowInput(KeyCode up, KeyCode down, KeyCode left, KeyCode right) {
            Corresponds = new Dictionary<GPArrow, KeyCode>()
                {
                    { GPArrow.Up    , up    },
                    { GPArrow.Down  , down  },
                    { GPArrow.Left  , left  },
                    { GPArrow.Right , right }
                };
        }
        #endregion

        #region getter
        public override bool Get    (GPArrow arrow) => UInput.GetKey(Corresponds[arrow]);
        public override bool GetDown(GPArrow arrow) => UInput.GetKeyDown(Corresponds[arrow]);
        public override bool GetUp  (GPArrow arrow) => UInput.GetKeyUp(Corresponds[arrow]);

        public override string ToString() {
            return $"Up:{Corresponds[GPArrow.Up]}, Down:{Corresponds[GPArrow.Down]}, Left:{Corresponds[GPArrow.Left]}, Right:{Corresponds[GPArrow.Right]}";
        }
        #endregion
    }


    /// <summary>
    /// Anyユーザ用方向入力対応表
    /// - ArrowInputを複数持ち、そのいずれかが発火していれば全体も発火
    /// </summary>
    public class AnyArrowInput : AbsArrowInput {
        #region field
        /// <value> 対応表の実体 </value>
        IEnumerable<IArrowInput> Arrows { get; }
        #endregion

        #region ctor
        public AnyArrowInput(IEnumerable<IArrowInput> arrows) { Arrows = arrows; }
        #endregion

        #region getter
        public override bool Get    (GPArrow arrow) => Arrows.Any(i => i.Get(arrow));
        public override bool GetDown(GPArrow arrow) => Arrows.Any(i => i.Get(arrow));
        public override bool GetUp  (GPArrow arrow) => Arrows.Any(i => i.Get(arrow));
        #endregion
    }

}

