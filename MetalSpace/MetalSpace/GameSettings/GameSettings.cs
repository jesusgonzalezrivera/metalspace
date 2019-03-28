using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MetalSpace.Events;

namespace MetalSpace.Settings
{
    /// <summary>
    /// Language of the user system.
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// Spanish language.
        /// </summary>
        Spanish,
        /// <summary>
        /// English language.
        /// </summary>
        English
    }

    /// <summary>
    /// Difficult that the user want to play.
    /// </summary>
    public enum Difficult
    {
        /// <summary>
        /// Easy difficult.
        /// </summary>
        Easy,
        /// <summary>
        /// Normal difficult.
        /// </summary>
        Normal,
        /// <summary>
        /// Hard difficult.
        /// </summary>
        Hard
    }

    /// <summary>
    /// The <c>GameSettings</c> class represents all the options that the user
    /// can modify in the <c>OptionsMenu</c> screen. It includes game, keyboard,
    /// graphic and sound options.
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        #region Game Fields

        /// <summary>
        /// Store for the GameLanguage property.
        /// </summary>
        private Language _gameLanguage = Language.English;

        /// <summary>
        /// Store for the Difficult property.
        /// </summary>
        private Difficult _difficult = Difficult.Easy;

        #endregion

        #region Game Properties

        /// <summary>
        /// GameLanguage property
        /// </summary>
        /// <value>
        /// Language used in the game (menus, message box...).
        /// </value>
        public Language GameLanguage
        {
            get { return _gameLanguage; }
            set { _gameLanguage = value; }
        }

        /// <summary>
        /// Difficult property
        /// </summary>
        /// <value>
        /// Difficult of the enemies and characteristic of the main player.
        /// </value>
        public Difficult Difficult
        {
            get { return _difficult; }
            set { _difficult = value; }
        }

        #endregion

        #region Keyboard Fields

        /// <summary>
        /// Store for the StartKey property.
        /// </summary>
        private Keys _startKey = Keys.Enter;

        /// <summary>
        /// Store for the SelectKey property.
        /// </summary>
        private Keys _selectKey = Keys.Back;

        /// <summary>
        /// Store for the LeftKey property.
        /// </summary>
        private Keys _leftKey = Keys.A;

        /// <summary>
        /// Store for the RightKey property.
        /// </summary>
        private Keys _rightKey = Keys.D;

        /// <summary>
        /// Store for the DownKey property.
        /// </summary>
        private Keys _downKey = Keys.S;

        /// <summary>
        /// Store for the UpKey property.
        /// </summary>
        private Keys _upKey = Keys.W;

        /// <summary>
        /// Store for the ActionKey property.
        /// </summary>
        private Keys _actionKey = Keys.E;

        /// <summary>
        /// Store for the JumpKey property.
        /// </summary>
        private Keys _jumpKey = Keys.Space;

        /// <summary>
        /// Store for the AttackKey property.
        /// </summary>
        private Keys _attackKey = Keys.K;

        /// <summary>
        /// Store for the SAttackKey property.
        /// </summary>
        private Keys _sAttackKey = Keys.L;

        /// <summary>
        /// Store for the InventoryKey property.
        /// </summary>
        private Keys _inventoryKey = Keys.I;

        /// <summary>
        /// Store for the CancelKey property.
        /// </summary>
        private Keys _cancelKey = Keys.Escape;

        #endregion

        #region Keyboard Properties

        /// <summary>
        /// StartKey property
        /// </summary>
        /// <value>
        /// Key used for the Start action.
        /// </value>
        public Keys StartKey
        {
            get { return _startKey; }
            set
            {
                if (_startKey != value)
                    _needSave = true;
                _startKey = value;
            }
        }

        /// <summary>
        /// SelectKey property
        /// </summary>
        /// <value>
        /// Key used for the Select action.
        /// </value>
        public Keys SelectKey
        {
            get { return _selectKey; }
            set
            {
                if (_selectKey != value)
                    _needSave = true;
                _selectKey = value;
            }
        }

        /// <summary>
        /// LeftKey property
        /// </summary>
        /// <value>
        /// Key used for the Left action.
        /// </value>
        public Keys LeftKey
        {
            get { return _leftKey; }
            set
            {
                if (_leftKey != value)
                    _needSave = true;
                _leftKey = value;
            }
        }

        /// <summary>
        /// RightKey property
        /// </summary>
        /// <value>
        /// Key used for the Right action.
        /// </value>
        public Keys RightKey
        {
            get { return _rightKey; }
            set
            {
                if (_rightKey != value)
                    _needSave = true;
                _rightKey = value;
            }
        }

        /// <summary>
        /// DownKey property
        /// </summary>
        /// <value>
        /// Key used for the Down action.
        /// </value>
        public Keys DownKey
        {
            get { return _downKey; }
            set
            {
                if (_downKey != value)
                    _needSave = true;
                _downKey = value;
            }
        }

        /// <summary>
        /// UpKey property
        /// </summary>
        /// <value>
        /// Key used for the Up action.
        /// </value>
        public Keys UpKey
        {
            get { return _upKey; }
            set
            {
                if (_upKey != value)
                    _needSave = true;
                _upKey = value;
            }
        }

        /// <summary>
        /// ActionKey property
        /// </summary>
        /// <value>
        /// Key used for the Action action.
        /// </value>
        public Keys ActionKey
        {
            get { return _actionKey; }
            set
            {
                if (_actionKey != value)
                    _needSave = true;
                _actionKey = value;
            }
        }

        /// <summary>
        /// JumpKey property
        /// </summary>
        /// <value>
        /// Key used for the Jump action.
        /// </value>
        public Keys JumpKey
        {
            get { return _jumpKey; }
            set
            {
                if (_jumpKey != value)
                    _needSave = true;
                _jumpKey = value;
            }
        }

        /// <summary>
        /// AttackKey property
        /// </summary>
        /// <value>
        /// Key used for the Attack action.
        /// </value>
        public Keys AttackKey
        {
            get { return _attackKey; }
            set
            {
                if (_attackKey != value)
                    _needSave = true;
                _attackKey = value;
            }
        }

        /// <summary>
        /// SAttackKey property
        /// </summary>
        /// <value>
        /// Key used for the SAttack action.
        /// </value>
        public Keys SAttackKey
        {
            get { return _sAttackKey; }
            set
            {
                if (_sAttackKey != value)
                    _needSave = true;
                _sAttackKey = value;
            }
        }

        /// <summary>
        /// InventoryKey property
        /// </summary>
        /// <value>
        /// Key used for the Inventory action.
        /// </value>
        public Keys InventoryKey
        {
            get { return _inventoryKey; }
            set
            {
                if (_inventoryKey != value)
                    _needSave = true;
                _inventoryKey = value;
            }
        }

        /// <summary>
        /// CancelKey property
        /// </summary>
        /// <value>
        /// Key used for the Cancel action.
        /// </value>
        public Keys CancelKey
        {
            get { return _cancelKey; }
            set
            {
                if (_cancelKey != value)
                    _needSave = true;
                _cancelKey = value;
            }
        }

        #endregion

        #region Graphic Fields
        
        /// <summary>
        /// Store for the virtual ResolutionWidth property.
        /// </summary>
        private int _resolutionWidth = 1024;

        /// <summary>
        /// Store for the virtual ResolutionHeight property.
        /// </summary>
        private int _resolutionHeight = 768;

        /// <summary>
        /// Minimum windows width resolution.
        /// </summary>
        public const int MinimumWindowsResolutionWidth = 640;

        /// <summary>
        /// Store for the WindowsResolutionWidth property.
        /// </summary>
        private int _windowsResolutionWidth = 800;

        /// <summary>
        /// Minimum windows height resolution.
        /// </summary>
        public const int MinimumWindowsResolutionHeight = 480;

        /// <summary>
        /// Store for the WindowsResolutionHeight property.
        /// </summary>
        private int _windowsResolutionHeight = 600;

        /// <summary>
        /// Store for the FullScreen property.
        /// </summary>
        private bool _fullScreen = false;

        /// <summary>
        /// Store for the VerticalSyncronization property.
        /// </summary>
        private bool _verticalSyncronization = false;

        /// <summary>
        /// Store for the HighDetail property.
        /// </summary>
        private bool _highDetail = false;

        #endregion

        #region Graphic Properties

        /// <summary>
        /// ResolutionWidth property
        /// </summary>
        /// <value>
        /// Virtual width resolution used in the game.
        /// </value>
        public int ResolutionWidth
        {
            get { return _resolutionWidth; }
        }

        /// <summary>
        /// ResolutionHeight property
        /// </summary>
        /// <value>
        /// Virtual height resolution used in the game.
        /// </value>
        public int ResolutionHeight
        {
            get { return _resolutionHeight; }
        }

        /// <summary>
        /// WindowsResolutionWidth property
        /// </summary>
        /// <value>
        /// Windows width resolution used in the game.
        /// </value>
        public int WindowsResolutionWidth
        {
            get { return _windowsResolutionWidth; }
            set
            {
                if (_windowsResolutionWidth != value)
                    _needSave = true;
                _windowsResolutionWidth = value;
            }
        }

        /// <summary>
        /// WindowsResolutionHeight property
        /// </summary>
        /// <value>
        /// Windows height resolution used in the game.
        /// </value>
        public int WindowsResolutionHeight
        {
            get { return _windowsResolutionHeight; }
            set
            {
                if (_windowsResolutionHeight != value)
                    _needSave = true;
                _windowsResolutionHeight = value;
            }
        }

        /// <summary>
        /// FullScreen property
        /// </summary>
        /// <value>
        /// true if the game uses full screen mode, false otherwise.
        /// </value>
        public bool FullScreen
        {
            get { return _fullScreen; }
            set
            {
                if (_fullScreen != value)
                    _needSave = true;
                _fullScreen = value;
            }
        }

        /// <summary>
        /// VerticalSyncronization property
        /// </summary>
        /// <value>
        /// true if the game uses vertical syncronization, false otherwise.
        /// </value>
        public bool VerticalSyncronization
        {
            get { return _verticalSyncronization; }
            set 
            {
                if (_verticalSyncronization != value)
                    _needSave = true;
                _verticalSyncronization = value; 
            }
        }

        /// <summary>
        /// VerticalSyncronization property
        /// </summary>
        /// <value>
        /// true if the game uses high detail effects, false otherwise.
        /// </value>
        public bool HighDetail
        {
            get { return _highDetail; }
            set
            {
                if (_highDetail != value)
                    _needSave = true;
                _highDetail = value;
            }
        }

        #endregion

        #region Sound Fields

        /// <summary>
        /// Store for the SoundVolume property.
        /// </summary>
        private float _soundVolume = 1.0f;

        /// <summary>
        /// Store for the MusicVolume property.
        /// </summary>
        private float _musicVolume = 1.0f;

        #endregion

        #region Sound Properties

        /// <summary>
        /// SoundVolume property
        /// </summary>
        /// <value>
        /// Volume used in the game effects (enemies, players, shots...).
        /// </value>
        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                if (_soundVolume != value)
                    _needSave = true;
                _soundVolume = value;
            }
        }

        /// <summary>
        /// MusicVolume property
        /// </summary>
        /// <value>
        /// Volume used in the music of the game.
        /// </value>
        public float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                if (_musicVolume != value)
                    _needSave = true;
                _musicVolume = value;
            }
        }

        #endregion

        #region Default

        /// <summary>
        /// true if the settings have been modified and need to be saved.
        /// </summary>
        private static bool _needSave = false;

        /// <summary>
        /// Name of the file that contains the settings.
        /// </summary>
        private const string SettingsFilename = "GameSetting.xml";

        /// <summary>
        /// Store for the DefaultInstance property.
        /// </summary>
        private static GameSettings _defaultInstance = null;

        /// <summary>
        /// DefaultInstance property
        /// </summary>
        /// <value>
        /// Default instance used for the settings.
        /// </value>
        public static GameSettings DefaultInstance
        {
            get { return _defaultInstance; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>GameSettings</c> class.
        /// </summary>
        private GameSettings() { }

        #endregion

        #region Initialize Method

        public static void Initialize()
        {
            _defaultInstance = new GameSettings();
            Load();
            //_needSave = true;
            //Save();
        }

        #endregion

        #region Load Method
        
        /// <summary>
        /// Load the settings of the user from the SettingsFilename.
        /// </summary>
        public static void Load()
        {
            _needSave = false;

            Stream file = FileHelper.LoadGameContentFile(SettingsFilename);
            if (file == null)
            {
                _needSave = true;
                return;
            }

            if (file.Length == 0)
            {
                file.Close();

                file = FileHelper.LoadGameContentFile(SettingsFilename);
                if (file != null)
                {
                    GameSettings loadedGameSetting =
                        (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(file);
                    if (loadedGameSetting != null)
                    { }
                }

                _needSave = true;
                Save();
            }
            else
            {
                GameSettings loadedGameSetting =
                    (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(file);
                if (loadedGameSetting != null)
                    _defaultInstance = loadedGameSetting;

                file.Close();
            }
        }

        #endregion

        #region Save Method

        /// <summary>
        /// Save the current options of the user.
        /// </summary>
        public static void Save()
        {
            if (!_needSave)
                return;

            _needSave = false;

            Stream file = FileHelper.SaveGameContentFile(SettingsFilename);
            new XmlSerializer(typeof(GameSettings)).Serialize(file, DefaultInstance);
            file.Close();
        }

        #endregion
    }
}
