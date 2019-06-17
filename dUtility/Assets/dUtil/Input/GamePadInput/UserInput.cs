
using System.Collections.Generic;



namespace du {

    namespace di {


        public interface IUserInput {

            IArrowInput Arrow { get; }
            IButtonInput Button { get; }

            string ToString();

        }

        public class UserInput : IUserInput {

            private readonly IArrowInput m_arrows = null;
            private readonly IButtonInput m_buttons = null;

            public IArrowInput Arrow { get { return m_arrows; } }
            public IButtonInput Button { get { return m_buttons;} }

            public UserInput(IArrowInput arrow, IButtonInput button) {
                m_arrows = arrow;
                m_buttons = button;
            }

            public override string ToString() {
                return string.Format("Arrow[{0}], Button[{1}]",
                    m_arrows, m_buttons);
            }

        }

        public class AnyUserInput : IUserInput {

            private readonly IArrowInput m_arrows = null;
            private readonly IButtonInput m_buttons = null;

            public IArrowInput Arrow { get { return m_arrows; } }
            public IButtonInput Button { get { return m_buttons;} }

            public AnyUserInput(List<IArrowInput> arrows, List<IButtonInput> buttons) {
                m_arrows = new AnyArrowInput(arrows);
                m_buttons = new AnyButtonInput(buttons);
            }

            public AnyUserInput(List<IUserInput> userInput) {
                List<IArrowInput> arrows = new List<IArrowInput>();
                List<IButtonInput> buttons = new List<IButtonInput>();
                for (int i = 0; i < userInput.Count; ++i) {
                    arrows.Add(userInput[i].Arrow);
                    buttons.Add(userInput[i].Button);
                }
                m_arrows = new AnyArrowInput(arrows);
                m_buttons = new AnyButtonInput(buttons);
            }


            public override string ToString() {
                return string.Format("Arrow:{0}, Button{1}", m_arrows, m_buttons);
            }

        }

    }

}
