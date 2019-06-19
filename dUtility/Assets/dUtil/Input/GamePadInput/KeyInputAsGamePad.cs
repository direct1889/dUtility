using UnityEngine;
using System.Collections.Generic;
using GamePadRaw = du.di.Id.GamePadRaw;

namespace du.di {

    /// <summary>
    /// ゲームパッド入力をキーボード入力でシミュレート
    /// -
    /// </summary>
    public static class KeyInputAsGamePad {
        #region field
        /// <summary> GP生IDごとのキー入力ベースGamePadシミュレータ </summary>
        static IDictionary<GamePadRaw, IUserKeyAsGP> m_input;
        #endregion

        #region public
        /// <summary> 初期化 </summary>
        public static void Initialize() {
            if (!(m_input is null)) { return; } // すでに初期化済みなら終了

            m_input = new Dictionary<GamePadRaw, IUserKeyAsGP>();
            // CSVファイルから対応を読み込む
            using (var reader = new File.CSVReader<UserKeyAsGPDesc>("Utility/KeyInput4GamePad", true))
            {
                foreach (var desc in reader) {
                    m_input.Add(desc.GPRawID, desc.Generate());
                }
            }
            m_input.Add(GamePadRaw.Any, new AnyUserKeyAsGP(m_input.Values));
        }
        #endregion

        #region getter
        /// <summary> GP生IDからユーザ入力対応表を引く </summary>
        public static IUserKeyAsGP User(GamePadRaw rawID) => m_input[rawID];
        #endregion

        #region private
        /// <summary>
        /// 単一ユーザのゲームパッド入力をキーボード入力でシミュレート
        /// - CSVからどのキーをどのボタンとして扱うかの対応表を生成する
        /// </summary>
        private class UserKeyAsGPDesc {
            #region field
            /// <summary> ゲームパッドの生ID </summary>
            [File.CSVColAttr(0)] GamePadRaw gpRawID;

            /// <summary> 十字キー上 </summary>
            [File.CSVColAttr(1)] KeyCode up;
            /// <summary> 十字キー下 </summary>
            [File.CSVColAttr(2)] KeyCode down;
            /// <summary> 十字キー左 </summary>
            [File.CSVColAttr(3)] KeyCode left;
            /// <summary> 十字キー右 </summary>
            [File.CSVColAttr(4)] KeyCode right;

            /// <summary> 〇ボタン </summary>
            [File.CSVColAttr(5)] KeyCode circle;
            /// <summary> ×ボタン </summary>
            [File.CSVColAttr(6)] KeyCode cross;
            /// <summary> △ボタン </summary>
            [File.CSVColAttr(7)] KeyCode triangle;
            /// <summary> □ボタン </summary>
            [File.CSVColAttr(8)] KeyCode square;
            /// <summary> スタートボタン </summary>
            [File.CSVColAttr(9)] KeyCode start;

            /// <summary> L2ボタン </summary>
            [File.CSVColAttr(10)] KeyCode l2;
            /// <summary> L1ボタン </summary>
            [File.CSVColAttr(11)] KeyCode l1;
            /// <summary> R1ボタン </summary>
            [File.CSVColAttr(12)] KeyCode r1;
            /// <summary> R2ボタン </summary>
            [File.CSVColAttr(13)] KeyCode r2;
            /// <summary> L3ボタン </summary>
            [File.CSVColAttr(14)] KeyCode l3;
            /// <summary> R3ボタン </summary>
            [File.CSVColAttr(15)] KeyCode r3;
            #endregion

            #region public
            /// <summary> ゲームパッドの生ID </summary>
            public GamePadRaw GPRawID => gpRawID;

            /// <summary> 対応表(IUserInput)を生成 </summary>
            public IUserKeyAsGP Generate() {
                IArrowKeyAsGP arrow =
                    new ArrowKeyAsGP(up, down, left, right);
                IButtonKeyAsGP button =
                    new ButtonKeyAsGP(
                        new Dictionary<GPButton, KeyCode>(){
                            { GPButton.Triangle, triangle },
                            { GPButton.Circle  , circle   },
                            { GPButton.Cross   , cross    },
                            { GPButton.Square  , square   },
                            { GPButton.Start   , start    },
                            { GPButton.RightShoulder1, r1 },
                            { GPButton.LeftShoulder1 , l1 },
                            { GPButton.RightShoulder2, r2 },
                            { GPButton.LeftShoulder2 , l2 },
                            { GPButton.RightStick    , r3 },
                            { GPButton.LeftStick     , l3 }
                        });
                return new UserKeyAsGP(arrow, button);
            }
            #endregion
        }
        #endregion
    }

}
