
using UnityEngine;

using du.di;
using GamePadID = du.di.Id.GamePad;
using static du.di.Id.IdExtension;
using static du.Ex.ExVector;
using GPRawInput = GamepadInput.GamePad;


public class DemoSensor : MonoBehaviour {

    //! ----- field -----
    #region field

    [SerializeField] GamePadID m_gpId = GamePadID.Any;

    [SerializeField] GameObject m_circle    = null;
    [SerializeField] GameObject m_cross        = null;
    [SerializeField] GameObject m_triangle    = null;
    [SerializeField] GameObject m_square    = null;

    [SerializeField] GameObject m_dLeft        = null;
    [SerializeField] GameObject m_dRight    = null;
    [SerializeField] GameObject m_dUp        = null;
    [SerializeField] GameObject m_dDown        = null;

    [SerializeField] GameObject m_l1 = null;
    [SerializeField] GameObject m_r1 = null;
    [SerializeField] GameObject m_l2 = null;
    [SerializeField] GameObject m_r2 = null;

    [SerializeField] GameObject m_l3 = null;
    [SerializeField] GameObject m_r3 = null;

    [SerializeField] RectTransform m_stickL    = null;
    [SerializeField] RectTransform m_stickR    = null;

    #endregion


    //! ----- mono behaviour -----
    #region mono behaviour

    private void Start() {}

    private void Update() {

        m_circle    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.Circle    ));
        m_cross        ?.SetActive(InputManager.GetButton(m_gpId, GPButton.Cross    ));
        m_triangle    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.Triangle));
        m_square    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.Square    ));

        m_l1    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.LeftShoulder1    ));
        m_r1    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.RightShoulder1    ));
        m_l2    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.LeftShoulder2    ));
        m_r2    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.RightShoulder2    ));

        m_l3    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.LeftStick    ));
        m_r3    ?.SetActive(InputManager.GetButton(m_gpId, GPButton.RightStick    ));

        {
            Vector2 dpad = InputManager.GetArrowDPadVec2(m_gpId);
            m_dLeft    ?.SetActive(dpad.x <= -1.0f);
            m_dRight?.SetActive(dpad.x >=  1.0f);
            m_dUp    ?.SetActive(dpad.y >=  1.0f);
            m_dDown    ?.SetActive(dpad.y <= -1.0f);
        }
        {
            float factor = 12.0f;
            m_stickL.localPosition =
                GPRawInput.GetAxis(GPAxis.LeftStick, m_gpId.ToRawID())
                .AddZ(0.0f) * factor;
            m_stickR.localPosition =
                GPRawInput.GetAxis(GPAxis.RightStick, m_gpId.ToRawID())
                .AddZ(0.0f) * factor;
        }

    }

    #endregion


}
