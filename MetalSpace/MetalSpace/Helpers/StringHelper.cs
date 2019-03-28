using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;
using MetalSpace.Settings;
using MetalSpace.Managers;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>StringHelper</c> class represents that contains the subtitles
    /// used in the game. It also permits to load and save new subtitles.
    /// </summary>
    [XmlType(Namespace = "")]
    class StringHelper
    {
        #region Fields

        /// <summary>
        /// Root name of the used to store the subtitles. 
        /// <example>StringsSpanish, StringsEnglish</example>
        /// </summary>
        private const string StringFileName = "Strings";

        /// <summary>
        /// Store for the Strings property.
        /// </summary>
        private static Hashtable _strings;

        /// <summary>
        /// Store for the DefaultInstance property.
        /// </summary>
        private static StringHelper _defaultInstance = null;

        #endregion

        #region Properties

        /// <summary>
        /// Strings property
        /// </summary>
        /// <value>
        /// Hashtable that contains the subtitles used in the game.
        /// </value>
        public static Hashtable Strings
        {
            get { return _strings; }
            set { _strings = value; }
        }

        /// <summary>
        /// DefaultInstance property
        /// </summary>
        /// <value>
        /// The default instance used by to store the strings.
        /// </value>
        public static StringHelper DefaultInstance
        {
            get { return _defaultInstance; }
            set { _defaultInstance = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Contructor of the <c>StringHelper</c> class.
        /// </summary>
        public StringHelper() { }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initilize the elements used by the <c>StringHelper</c>.
        /// </summary>
        public static void Initialize()
        {
            _defaultInstance = new StringHelper();
            _strings = new Hashtable();
            _defaultInstance.LoadStrings();
            //_defaultInstance.SaveStrings();
        }

        #endregion

        #region Get Function

        /// <summary>
        /// Get the string with the id specified.
        /// </summary>
        /// <param name="id">Id of the string.</param>
        /// <returns>String with the id specified.</returns>
        public string Get(string id)
        {
            return (string)_strings[id];
        }

        #endregion

        #region LoadStrings Method

        /// <summary>
        /// Load the list of strings.
        /// </summary>
        private void LoadStrings()
        {
            string fullName = StringFileName + GameSettings.DefaultInstance.GameLanguage + ".xml";
            
            // Open the file that contains the log messages
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader xr = XmlReader.Create("./Strings/" + fullName, settings);
            XmlSerializer xs = new XmlSerializer(typeof(ListNameValuePair));

            ListNameValuePair orderedTable = (ListNameValuePair) xs.Deserialize(xr);
            
            _strings = orderedTable.ConvertIntoHashTable();

            xr.Close();
        }

        #endregion

        #region SaveStrings Method

        /// <summary>
        /// Save the list of strings.
        /// </summary>
        private void SaveStrings()
        {
            Hashtable table = new Hashtable();
            /*table.Add("menu_continue", "Continue");
            table.Add("menu_new", "New game");
            table.Add("menu_load", "Load game");
            table.Add("menu_options", "Options");
            table.Add("menu_exit", "Exit");

            table.Add("menu_option_categories_video", "Video");
            table.Add("menu_option_category_video_resolution", "Resolution");
            table.Add("menu_option_category_video_fullscreen", "Fullscreen");
            table.Add("menu_option_category_video_vertical_syncronization", "Vertical Sync.");
            table.Add("menu_option_category_video_quality", "Quality");

            table.Add("menu_option_categories_sound", "Sound");
            table.Add("menu_option_category_sound_music_volume", "Music vol.");
            table.Add("menu_option_category_sound_effects_volume", "Effects vol.");

            table.Add("menu_option_categories_pad", "Controls");
            table.Add("menu_option_category_pad_up", "Up");
            table.Add("menu_option_category_pad_down", "Down");
            table.Add("menu_option_category_pad_left", "Left");
            table.Add("menu_option_category_pad_right", "Right");
            table.Add("menu_option_category_pad_start", "Start");
            table.Add("menu_option_category_pad_select", "Select");
            table.Add("menu_option_category_pad_action", "Action");
            table.Add("menu_option_category_pad_jump", "Jump");
            table.Add("menu_option_category_pad_attack", "Attack");
            table.Add("menu_option_category_pad_sattack", "Spec. attack");

            table.Add("menu_load_empty", "No saved any game");

            table.Add("menu_exit_message", "Are you sure you want to exit?");
            
            table.Add("loading_screen_message", "Loading...");

            table.Add("activated", "On");
            table.Add("non_activated", "Off");

            table.Add("quality_high", "High");
            table.Add("quality_low", "Low");

            table.Add("difficult_easy", "Easy");
            table.Add("difficult_normal", "Normal");
            table.Add("difficult_hard", "Hard");
            
            table.Add("accept", "Accept");
            table.Add("cancel", "Cancel");

            table.Add("menu_ingame_continue", "Continue");
            table.Add("menu_ingame_save", "Save game");
            table.Add("menu_ingame_lastsave", "Last save point");
            table.Add("menu_ingame_options", "Options");
            table.Add("menu_ingame_exit", "Exit");

            table.Add("equipment_armature", "Armature");
            table.Add("equipment_armature_defense", "Defense");
            table.Add("equipment_armature_skill", "Skill");
            table.Add("equipment_armature_type", "Type");
            table.Add("equipment_armature_type_helmet", "Helmet");
            table.Add("equipment_armature_type_suit", "Suit");
            table.Add("equipment_armature_type_gloves", "Gloves");
            table.Add("equipment_armature_type_boots", "Boots");

            table.Add("equipment_weapon", "Weapon");
            table.Add("equipment_weapon_type", "Type");
            table.Add("equipment_weapon_ammo", "Ammo");
            table.Add("equipment_weapon_power", "Power");

            table.Add("equipment_select_object_unequip", "Unequip");
            table.Add("equipment_select_object_throw_away", "Throw away");

            table.Add("inventory_select_object_equip", "Equip");
            table.Add("inventory_select_object_throw_away", "Throw away");
            table.Add("inventory_select_object_reload", "Reload");

            table.Add("inventory_total_points", "Total points: ");
            table.Add("inventory_material_aerogel", "Aerogel: ");
            table.Add("inventory_material_debolio", "Debolio: ");
            table.Add("inventory_material_fulereno", "Fulereno: ");
             
            table.Add("dead_screen_message", "Do you want to continue?");
            table.Add("dead_screen_yes", "Yes");
            table.Add("dead_screen_no", "No");*/

            table.Add("menu_continue", "Continuar");
            table.Add("menu_new", "Nuevo juego");
            table.Add("menu_load", "Cargar partida");
            table.Add("menu_options", "Opciones");
            table.Add("menu_exit", "Salir");
            
            table.Add("menu_option_categories_video", "Video");
            table.Add("menu_option_category_video_resolution", "Resolucion");
            table.Add("menu_option_category_video_fullscreen", "Pant. completa");
            table.Add("menu_option_category_video_vertical_syncronization", "Sinc. vertical");
            table.Add("menu_option_category_video_quality", "Calidad");

            table.Add("menu_option_categories_sound", "Sonido");
            table.Add("menu_option_category_sound_music_volume", "Vol. musica");
            table.Add("menu_option_category_sound_effects_volume", "Vol. efectos");

            table.Add("menu_option_categories_pad", "Controles");
            table.Add("menu_option_category_pad_up", "Arriba");
            table.Add("menu_option_category_pad_down", "Abajo");
            table.Add("menu_option_category_pad_left", "Izquierda");
            table.Add("menu_option_category_pad_right", "Derecha");
            table.Add("menu_option_category_pad_start", "Start");
            table.Add("menu_option_category_pad_select", "Select");
            table.Add("menu_option_category_pad_action", "Accion");
            table.Add("menu_option_category_pad_jump", "Saltar");
            table.Add("menu_option_category_pad_attack", "Ataque");
            table.Add("menu_option_category_pad_sattack", "Ataque esp.");

            table.Add("menu_load_empty", "No se ha guardado ninguna partida");
            
            table.Add("menu_exit_message", "Estas seguro de que deseas salir?");

            table.Add("loading_screen_message", "Cargando...");

            table.Add("activated", "Si");
            table.Add("non_activated", "No");
            
            table.Add("quality_high", "Alta");
            table.Add("quality_low", "Baja");

            table.Add("difficult_easy", "Facil");
            table.Add("difficult_normal", "Normal");
            table.Add("difficult_hard", "Dificil");

            table.Add("accept", "Aceptar");
            table.Add("cancel", "Cancelar");

            table.Add("menu_ingame_continue", "Continuar");
            table.Add("menu_ingame_save", "Guardar partida");
            table.Add("menu_ingame_lastsave", "Ultimo pt. guard.");
            table.Add("menu_ingame_options", "Opciones");
            table.Add("menu_ingame_exit", "Salir");

            table.Add("equipment_armature", "Armadura");
            table.Add("equipment_armature_defense", "Defensa");
            table.Add("equipment_armature_skill", "Destreza");
            table.Add("equipment_armature_type", "Tipo");
            table.Add("equipment_armature_type_helmet", "Casco");
            table.Add("equipment_armature_type_suit", "Traje");
            table.Add("equipment_armature_type_gloves", "Guantes");
            table.Add("equipment_armature_type_boots", "Botas");

            table.Add("equipment_weapon", "Arma");
            table.Add("equipment_weapon_type", "Tipo");
            table.Add("equipment_weapon_ammo", "Municion");
            table.Add("equipment_weapon_power", "Potencia");

            table.Add("equipment_select_object_unequip", "Desequipar");
            table.Add("equipment_select_object_throw_away", "Tirar");

            table.Add("inventory_select_object_equip", "Equipar");
            table.Add("inventory_select_object_throw_away", "Tirar");
            table.Add("inventory_select_object_reload", "Recargar");

            table.Add("inventory_total_points", "Total puntos: ");
            table.Add("inventory_material_aerogel", "Aerogel: ");
            table.Add("inventory_material_debolio", "Debolio: ");
            table.Add("inventory_material_fulereno", "Fulereno: ");

            table.Add("dead_screen_message", "Quieres continuar?");
            table.Add("dead_screen_yes", "Si");
            table.Add("dead_screen_no", "No");

            ListNameValuePair list = new ListNameValuePair(table);

            XmlSerializer serializer = new XmlSerializer(typeof(ListNameValuePair));
            
            StreamWriter writer = new StreamWriter("StringsSpanish1.xml");

            serializer.Serialize(writer, list);

            writer.Close();
            writer.Dispose();
        }

        #endregion
    }
}
