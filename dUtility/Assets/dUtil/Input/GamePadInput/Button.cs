
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace du {


    namespace di {


        public interface IButtonInput {

            bool Get(GPButton button);
            bool GetDown(GPButton button);
            bool GetUp(GPButton button);

            float GetF(GPButton button);
            float GetDownF(GPButton button);
            float GetUpF(GPButton button);


            string ToString();

        }


        public class ButtonInput : IButtonInput {

            private Dictionary<GPButton, KeyCode> m_keys = null;


            public ButtonInput(Dictionary<GPButton, KeyCode> map) {
                m_keys = map;
            }


            public bool Get(GPButton button) {
                return m_keys.ContainsKey(button)
                    && UnityEngine.Input.GetKey(m_keys[button]);
            }

            public bool GetDown(GPButton button) {
                return m_keys.ContainsKey(button)
                    && UnityEngine.Input.GetKeyDown(m_keys[button]);
            }

            public bool GetUp(GPButton button) {
                return m_keys.ContainsKey(button)
                    && UnityEngine.Input.GetKeyUp(m_keys[button]);
            }


            public float GetF(GPButton button) {
                return System.Convert.ToInt32(
                    m_keys.ContainsKey(button)
                    && UnityEngine.Input.GetKey(m_keys[button])
                    );
            }

            public float GetDownF(GPButton button) {
                return System.Convert.ToInt32(
                    m_keys.ContainsKey(button)
                    && UnityEngine.Input.GetKeyDown(m_keys[button])
                    );
            }

            public float GetUpF(GPButton button) {
                return System.Convert.ToInt32(
                    m_keys.ContainsKey(button)
                    && UnityEngine.Input.GetKeyUp(m_keys[button])
                    );
            }



            public override string ToString() {
                return string.Format("Two:{0}, Three:{1}, START:{2}",
                    m_keys[GPButton.Circle],
                    m_keys[GPButton.Cross],
                    m_keys[GPButton.Start]);
            }

        }


        public class AnyButtonInput : IButtonInput {

            private List<IButtonInput> m_buttons = new List<IButtonInput>();

            public AnyButtonInput(List<IButtonInput> buttons) {
                m_buttons = buttons;
            }


            public bool Get(GPButton button) {
                return m_buttons.Any(i => i.Get(button));
            }

            public bool GetDown(GPButton button) {
                return m_buttons.Any(i => i.GetDown(button));
            }

            public bool GetUp(GPButton button) {
                return m_buttons.Any(i => i.GetUp(button));
            }


            public float GetF(GPButton button) {
                return System.Convert.ToInt32(Get(button));
            }

            public float GetDownF(GPButton button) {
                return System.Convert.ToInt32(GetDown(button));
            }

            public float GetUpF(GPButton button) {
                return System.Convert.ToInt32(GetUp(button));
            }


        }


    }


}
