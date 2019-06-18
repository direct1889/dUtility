using UnityEngine;

using System.Collections.Generic;
using System.Linq;

using GamePadRaw = du.di.Id.GamePadRaw;


namespace du {

    namespace di {


        public class UserInputGenerator {


            //! ----- field -----
            #region field

            [File.CSVColAttr(0)] GamePadRaw gpRawID;

            [File.CSVColAttr(1)] KeyCode up;
            [File.CSVColAttr(2)] KeyCode down;
            [File.CSVColAttr(3)] KeyCode left;
            [File.CSVColAttr(4)] KeyCode right;

            [File.CSVColAttr(5)] KeyCode circle;    // 〇ボタン
            [File.CSVColAttr(6)] KeyCode cross;     // ×ボタン
            [File.CSVColAttr(7)] KeyCode triangle;  // △ボタン
            [File.CSVColAttr(8)] KeyCode square;    // □ボタン
            [File.CSVColAttr(9)] KeyCode start;  // スタートボタン

            [File.CSVColAttr(10)] KeyCode l2;
            [File.CSVColAttr(11)] KeyCode l1;
            [File.CSVColAttr(12)] KeyCode r1;
            [File.CSVColAttr(13)] KeyCode r2;
            [File.CSVColAttr(14)] KeyCode l3;
            [File.CSVColAttr(15)] KeyCode r3;

            #endregion


            //! ----- public -----
            #region public

            public GamePadRaw GPRawID { get { return gpRawID; } }

            public IUserInput Generate() {

                IArrowInput arrow =
                    new ArrowInput(gpRawID, up, down, left, right);

                IButtonInput button =
                    new ButtonInput(
                        new Dictionary<GPButton, KeyCode>(){
                            { GPButton.Triangle , triangle  },
                            { GPButton.Circle   , circle    },
                            { GPButton.Cross    , cross     },
                            { GPButton.Square   , square    },
                            { GPButton.Start    , start     },
                            { GPButton.RightShoulder1, r1   },
                            { GPButton.LeftShoulder1 , l1   },
                            { GPButton.RightShoulder2, r2   },
                            { GPButton.LeftShoulder2 , l2   },
                            { GPButton.RightStick     , r3   },
                            { GPButton.LeftStick     , l3   }
                        });

                return new UserInput(arrow, button);

            }

            #endregion

        }


        public static class KeyInput4GamePad {

            private static Dictionary<GamePadRaw, IUserInput> m_input = null;


            public static void Initialize() {

                if (m_input != null) { return; }


                m_input = new Dictionary<GamePadRaw, IUserInput>();
                //! ファイルからキーボードとゲームパッドの対応を読み込む
                using (var userGenerators = new File.CSVReader<UserInputGenerator>("Utility/KeyInput4GamePad", true, false))
                {
                    userGenerators.ToList().ForEach(
                        gen => m_input.Add(gen.GPRawID, gen.Generate())
                    );
                }

                m_input.Add(GamePadRaw.Any, new AnyUserInput(
                    new List<IUserInput>(){
                        m_input[GamePadRaw._1P],
                        m_input[GamePadRaw._2P],
                        m_input[GamePadRaw._3P],
                        m_input[GamePadRaw._4P],
                    }));

            }


            public static IUserInput User(GamePadRaw index) {

                try {
                    return m_input[index];
                }
                catch (System.Exception e) {
                    Debug.LogError(e + " | index = " + index + ", m_input = " + m_input.Count + ":" + m_input);
                    return m_input[GamePadRaw._1P];
                }

            }

        }


    }

}
