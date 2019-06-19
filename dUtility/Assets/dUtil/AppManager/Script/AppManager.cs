using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;


/// <summary> アプリケーション全体に関わる情報 </summary>
namespace du.App {

    /// <summary> マウスカーソルを表示するか </summary>
    public enum MouseCursorMode {
        Invisible, Visible, Detail
    }

    /// <summary> Audioに関する設定項目 </summary>
    [Serializable]
    public class AudioDesc {
        public bool isMute = true;
        public float masterVolume = 0.01f;
    }

    /// <summary> 実機での画面/ウィンドウ設定 </summary>
    [Serializable]
    public class ResolutionDesc {
        public int width;
        public int height;
        public bool isFullscreen = false;
        public int preferredRefreshRate = 60;

        public void SetResolution() {
            Screen.SetResolution(width, height, isFullscreen, preferredRefreshRate);
        }
    }


    public interface IAppManager {
        MouseCursorMode MouseCursorMode { get; }
    }

    /// <summary>
    /// アプリ全体に関わる基本的な設定
    /// - アプリ起動時に値が設定され、動的な変更は不能
    /// </summary>
    public class AppManager : SingletonMonoBehaviour<AppManager>, IAppManager {
        #region field
        /// <summary> AppManagerの起動が済み次第呼び出されるシーン名 </summary>
        [SerializeField] string m_pilotScene;
        /// <summary> Audioに関する設定項目 </summary>
        [SerializeField] AudioDesc m_audioDesc;
        /// <summary> 実機での画面/ウィンドウ設定 </summary>
        [SerializeField] ResolutionDesc m_resolutionDesc;
        /// <summary> デバッグ機能を有効化するか </summary>
        [SerializeField] bool m_isDebugMode = false;
        /// <summary> マウスカーソルを表示するか </summary>
        [SerializeField] MouseCursorMode m_mcmode = MouseCursorMode.Visible;
        #endregion

        #region getter property
        public MouseCursorMode MouseCursorMode => m_mcmode;
        #endregion

        #region mono
        private void Awake() {
            // アプリの終了まで生き続ける
            DontDestroyOnLoad(gameObject);
            // アプリ全体の初期化処理
            Boot();
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
        #endregion

        #region private
        /// <summary>
        /// アプリ全体の初期化
        /// - アプリ起動時に初期化子て欲しい機能はここから呼び出させる
        /// - これより先に初期化が必要な機能はつくらない
        /// </summary>
        private void Boot() {
            Debug.Log("Boot Apprication");
            m_resolutionDesc.SetResolution();
            InitializeDebugger();
            // di.RxTouchInput.Initialize();
            InitializeCursor();
            // DG.Tweening.DOTween.Init();
            Initialize_di();
            InitializeAudio();
            InitializeScene();
        }

        private void InitializeDebugger() {
            GetComponent<Test.LayerdLogMgr>().InitializeLLog();
            Test.DebugAssistant.Instance.gameObject.SetActive(m_isDebugMode);
        }
        private void InitializeCursor() {
            // Cursor.visible = m_mcmode == MouseCursorMode.Visible;
            Cursor.visible = m_mcmode != MouseCursorMode.Invisible;
            // OSUI.Instance.SetEnable(m_mcmode == MouseCursorMode.Detail);
        }
        private void Initialize_di() {
            di.GamePad.Initialize();
            di.Id.IdConverter.SetPlayer2GamePad(
                di.Id.GamePad._1P,
                di.Id.GamePad._2P,
                di.Id.GamePad._3P,
                di.Id.GamePad._4P
                );
        }
        private void InitializeAudio() {
            // utility.sound.SoundManager.Init();
            // utility.sound.SoundManager.BGM
            // .MasterVolumeSet(
            float volume = m_audioDesc.isMute ? 0f : m_audioDesc.masterVolume;
            // );
        }
        private void InitializeScene() {
            if (SceneManager.GetSceneByName(m_pilotScene).IsValid()) {
                if (Enumerable.Range(0, SceneManager.sceneCount)
                    .Select(SceneManager.GetSceneAt)
                    .All(scn => { return scn.name != m_pilotScene; }))
                {
                    SceneManager.LoadSceneAsync(m_pilotScene, LoadSceneMode.Additive);
                }
            }
        }
        #endregion

        #region static
        /// <summary> データファイルはここに置く </summary>
        /// <value> "Assets/MyData/" </value>
        public static string DataPath => Application.dataPath + "/MyData/";
        #endregion

    }

}
