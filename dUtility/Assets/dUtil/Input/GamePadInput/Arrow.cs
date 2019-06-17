
using UnityEngine;

using System.Collections.Generic;
using System.Linq;



namespace du {


    namespace di {


        public interface IArrowInput {

            bool Get(GPArrow arrow);
            bool GetDown(GPArrow arrow);
            bool GetUp(GPArrow arrow);

            float GetF(GPArrow arrow);
            float GetDownF(GPArrow arrow);
            float GetUpF(GPArrow arrow);

            Vector2 GetVector();
            Vector2 GetVectorDown();
            Vector2 GetVectorUp();


            string ToString();

        }


        public class ArrowInput : IArrowInput {

            private readonly Id.GamePadRaw m_index;
            private readonly Dictionary<GPArrow, KeyCode> m_keys = null;

            public ArrowInput(Id.GamePadRaw index, KeyCode up, KeyCode down, KeyCode left, KeyCode right) {

                m_keys = new Dictionary<GPArrow, KeyCode>()
                    {
                        { GPArrow.Up    , up    },
                        { GPArrow.Down  , down  },
                        { GPArrow.Left  , left  },
                        { GPArrow.Right , right }
                    };

            }


            public bool Get(GPArrow arrow) {
                return UnityEngine.Input.GetKey(m_keys[arrow]);
            }
            public bool GetDown(GPArrow arrow) {
                return UnityEngine.Input.GetKeyDown(m_keys[arrow]);
            }
            public bool GetUp(GPArrow arrow) {
                return UnityEngine.Input.GetKeyUp(m_keys[arrow]);
            }

            public float GetF(GPArrow arrow) {
                return System.Convert.ToInt32(UnityEngine.Input.GetKey(m_keys[arrow]));
            }
            public float GetDownF(GPArrow arrow) {
                return System.Convert.ToInt32(UnityEngine.Input.GetKeyDown(m_keys[arrow]));
            }
            public float GetUpF(GPArrow arrow) {
                return System.Convert.ToInt32(UnityEngine.Input.GetKeyUp(m_keys[arrow]));
            }

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


            public override string ToString() {
                return string.Format("Up:{0}, Down:{1}, Left:{2}, Right:{3}",
                    m_keys[GPArrow.Up],
                    m_keys[GPArrow.Down],
                    m_keys[GPArrow.Left],
                    m_keys[GPArrow.Right]);
            }

        }


        public class AnyArrowInput : IArrowInput {

            private List<IArrowInput> m_arrows = new List<IArrowInput>();

            public AnyArrowInput(List<IArrowInput> arrows) {
                m_arrows = arrows;
            }


            public bool Get(GPArrow arrow) {
                return m_arrows.Any(i => i.Get(arrow));
            }

            public bool GetDown(GPArrow arrow) {
                return m_arrows.Any(i => i.Get(arrow));
            }

            public bool GetUp(GPArrow arrow) {
                return m_arrows.Any(i => i.Get(arrow));
            }


            public float GetF(GPArrow arrow) {
                return System.Convert.ToInt32(m_arrows.Any(i => i.Get(arrow)));
            }

            public float GetDownF(GPArrow arrow) {
                return System.Convert.ToInt32(m_arrows.Any(i => i.Get(arrow)));
            }

            public float GetUpF(GPArrow arrow) {
                return System.Convert.ToInt32(m_arrows.Any(i => i.Get(arrow)));
            }

            public Vector2 GetVector() {
                return new Vector2(
                    GetF(GPArrow.Right) - GetF(GPArrow.Left),
                    GetF(GPArrow.Up   ) - GetF(GPArrow.Down)
                    );
            }

            public Vector2 GetVectorDown() {
                return new Vector2(
                    GetDownF(GPArrow.Right) - GetDownF(GPArrow.Left),
                    GetDownF(GPArrow.Up   ) - GetDownF(GPArrow.Down)
                    );
            }

            public Vector2 GetVectorUp() {
                return new Vector2(
                    GetUpF(GPArrow.Right) - GetUpF(GPArrow.Left),
                    GetUpF(GPArrow.Up   ) - GetUpF(GPArrow.Down)
                    );
            }


        }

    }

}

