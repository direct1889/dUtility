
using UnityEngine;

using GamePadID = du.di.Id.GamePad;
using static du.di.Id.ExId;
using static du.Ex.ExVector;
using GPImpl = GamepadInput.GamePadImpl;

namespace du.di {

    /// <summary> ゲームパッド入力のテスト表示 </summary>
    public class DemoSensor : MonoBehaviour {
        #region field
        [SerializeField] GamePadID m_gpId = GamePadID.Any;

        [SerializeField] GameObject m_circle   = null;
        [SerializeField] GameObject m_cross    = null;
        [SerializeField] GameObject m_triangle = null;
        [SerializeField] GameObject m_square   = null;

        [SerializeField] GameObject m_dLeft  = null;
        [SerializeField] GameObject m_dRight = null;
        [SerializeField] GameObject m_dUp    = null;
        [SerializeField] GameObject m_dDown  = null;

        [SerializeField] GameObject m_l1 = null;
        [SerializeField] GameObject m_r1 = null;
        [SerializeField] GameObject m_l2 = null;
        [SerializeField] GameObject m_r2 = null;

        [SerializeField] GameObject m_l3 = null;
        [SerializeField] GameObject m_r3 = null;

        [SerializeField] RectTransform m_stickL = null;
        [SerializeField] RectTransform m_stickR = null;
        #endregion

        #region mono behaviour
        private void Update() {
            // ボタン
            m_circle  ?.SetActive(GamePad.GetButton(m_gpId, GPButton.Circle        ));
            m_cross   ?.SetActive(GamePad.GetButton(m_gpId, GPButton.Cross         ));
            m_triangle?.SetActive(GamePad.GetButton(m_gpId, GPButton.Triangle      ));
            m_square  ?.SetActive(GamePad.GetButton(m_gpId, GPButton.Square        ));

            m_l1      ?.SetActive(GamePad.GetButton(m_gpId, GPButton.LeftShoulder1 ));
            m_r1      ?.SetActive(GamePad.GetButton(m_gpId, GPButton.RightShoulder1));
            m_l2      ?.SetActive(GamePad.GetButton(m_gpId, GPButton.LeftShoulder2 ));
            m_r2      ?.SetActive(GamePad.GetButton(m_gpId, GPButton.RightShoulder2));

            m_l3      ?.SetActive(GamePad.GetButton(m_gpId, GPButton.LeftStick     ));
            m_r3      ?.SetActive(GamePad.GetButton(m_gpId, GPButton.RightStick    ));

            { // 十字キー
                Vector2 dpad = GamePad.GetArrowDPadVec2(m_gpId);
                m_dLeft ?.SetActive(dpad.x <= -1.0f);
                m_dRight?.SetActive(dpad.x >=  1.0f);
                m_dUp   ?.SetActive(dpad.y >=  1.0f);
                m_dDown ?.SetActive(dpad.y <= -1.0f);
            }
            { // アナログスティック
                float factor = 12.0f;
                m_stickL.localPosition =
                    GPImpl.GetAxis(GPAxis.LeftStick, m_gpId.ToRawID())
                    .AddZ(0.0f) * factor;
                m_stickR.localPosition =
                    GPImpl.GetAxis(GPAxis.RightStick, m_gpId.ToRawID())
                    .AddZ(0.0f) * factor;
            }

        }
        #endregion
    }

}
