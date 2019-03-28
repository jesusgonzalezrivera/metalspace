using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using MetalSpace.GameScreens;
    
namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>FileHelper</c> class represents a group of functions that permits
    /// to the user to open, update and save files located in the workspace of the
    /// game.
    /// </summary>
    class FileHelper
    {
        #region Fields

        /// <summary>
        /// Store for the DefaultInstance property.
        /// </summary>
        private static FileHelper _defaultInstance = null;

        /// <summary>
        /// Storage device for user data, such as a memory unit or hard drive.
        /// </summary>
        private static StorageDevice _device = null;

        /// <summary>
        /// Logical collection of storage files.
        /// </summary>
        private static StorageContainer _container = null;

        #endregion

        #region Properties

        /// <summary>
        /// DefaultInstance property
        /// </summary>
        /// <value>
        /// Default instance used for the file helper.
        /// </value>
        public static FileHelper DefaultInstance
        {
            get { return _defaultInstance; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>FileHelper</c> class.
        /// </summary>
        public FileHelper() { }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialice the structures used in the <c>FileHelper</c> class.
        /// </summary>
        public static void Initialize()
        {
            _defaultInstance = new FileHelper();
            _defaultInstance.GetStorageDevice();
            _defaultInstance.GetStorageContainer();
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Initilize the StorageDevice instance of the <c>FileHelper</c> class.
        /// </summary>
        private void GetStorageDevice()
        {
            // Open storage device
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);

            // Wait for the WaitHandle to become signaled
            result.AsyncWaitHandle.WaitOne();

            if (result.IsCompleted)
                _device = StorageDevice.EndShowSelector(result);

            // Close the wait handle
            result.AsyncWaitHandle.Close();
        }

        /// <summary>
        /// Initilize the StorageContainer instance of the <c>FileHelper</c> class.
        /// </summary>
        private void GetStorageContainer()
        {
            // Open storage container
            IAsyncResult result = _device.BeginOpenContainer("MetalSpace", null, null);

            // Wait for the WaitHandle to become signaled
            result.AsyncWaitHandle.WaitOne();

            _container = _device.EndOpenContainer(result);

            // Close the wait handle
            result.AsyncWaitHandle.Close();
        }

        #endregion

        #region CreateGameContentFile

        /// <summary>
        /// Return a stream of a new game content file.
        /// </summary>
        /// <param name="fileName">Filename used for the game content file.</param>
        /// <param name="createNew">true if it is neccessary to create the content file, false otherwise.</param>
        /// <returns></returns>
        public static Stream CreateGameContentFile(string fileName, bool createNew)
        {
            return _container.OpenFile(fileName,
                createNew ? FileMode.Create : FileMode.OpenOrCreate,
                FileAccess.Write, FileShare.ReadWrite);
        }

        #endregion

        #region LoadGameContentFile

        /// <summary>
        /// Return a stream of a saved game content file.
        /// </summary>
        /// <param name="fileName">Filename used for the game content file.</param>
        /// <returns></returns>
        public static Stream LoadGameContentFile(string fileName)
        {
            if (_container.FileExists(fileName) == false)
                return null;
            else
                return _container.OpenFile(fileName, FileMode.Open, 
                    FileAccess.Read, FileShare.ReadWrite);
        }

        /// <summary>
        /// Return a stream of a saved game content file.
        /// </summary>
        /// <param name="fileName">Filename used for the game content file.</param>
        /// <param name="fileMode">Mode used to open the file.</param>
        /// <param name="fileAccess">Type of access used to open the file.</param>
        /// <param name="fileShare">Type of shared mode used to open the file.</param>
        /// <returns></returns>
        public static Stream LoadGameContentFile(string fileName, FileMode fileMode,
            FileAccess fileAccess, FileShare fileShare)
        {
            if (_container.FileExists(fileName) == false)
                return null;
            else
                return _container.OpenFile(fileName, fileMode, fileAccess, fileShare);
        }

        #endregion

        #region GetFilesInDirectory

        /// <summary>
        /// Get the list of files in the container directory.
        /// </summary>
        /// <returns>List of files in the container directory.</returns>
        public static string[] GetFilesInDirectory()
        {
            return _container.GetFileNames();
        }

        #endregion

        #region SaveGameContentFile

        /// <summary>
        /// Return a stream of a saved game content file.
        /// </summary>
        /// <param name="fileName">Filename used for the game content file.</param>
        /// <returns>Stream of a saved game content file.</returns>
        public static Stream SaveGameContentFile(string fileName)
        {
            return _container.OpenFile(fileName,
                FileMode.Create, FileAccess.Write);
        }

        #endregion

        #region OpenOrCreateFileForCurrentPlayer

        /// <summary>
        /// Open a new file to be used for the current player.
        /// </summary>
        /// <param name="fileName">Filename used for the current player file.</param>
        /// <param name="mode">Mode used to open the file.</param>
        /// <param name="access">Type of access used to open the file.</param>
        /// <returns>Stream for the current player file.</returns>
        public static Stream OpenFileForCurrentPlayer(string filename, 
            FileMode mode, FileAccess access)
        {
            return _container.OpenFile(filename, mode, access);
        }

        #endregion

        #region Get text lines
        
        /// <summary>
        /// Open the file specified and return an array of string with the 
        /// content of a file.
        /// </summary>
        /// <param name="filename">File to be readed.</param>
        /// <returns>Array of strings with the content of the file.</returns>
        static public string[] GetLines(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(
                    new FileStream(filename, FileMode.Open, FileAccess.Read),
                    System.Text.Encoding.UTF8);
                // Generic version
                List<string> lines = new List<string>();
                do
                {
                    lines.Add(reader.ReadLine());
                } while (reader.Peek() > -1);
                reader.Close();
                return lines.ToArray();
            }
            catch (FileNotFoundException)
            {
                // Failed to find a file.
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                // Failed to find a directory.
                return null;
            }
            catch (IOException)
            {
                // Something else must have happened.
                return null;
            }
        }

        #endregion

        #region ReadSavedGame Method

        /// <summary>
        /// Write a saved game of the player.
        /// </summary>
        /// <param name="name">Name of the saved game.</param>
        /// <param name="savedGame">Reference to the saved game to be saved.</param>
        public static void WriteSavedGame(string name, SavedGame savedGame)
        {
            Stream file = FileHelper.SaveGameContentFile(name);
            new XmlSerializer(typeof(SavedGame)).Serialize(file, savedGame);
            file.Close();
        }

        #endregion

        #region WriteSavedGame Method

        /// <summary>
        /// Read a saved game of the player.
        /// </summary>
        /// <param name="savedGameName">Name of the file that contains the saved game.</param>
        /// <returns>Instance of <c>SavedGame</c> with the content of the saved game.</returns>
        public static SavedGame ReadSavedGame(string savedGameName)
        {
            Stream file = FileHelper.LoadGameContentFile(savedGameName,
                    FileMode.Open, FileAccess.Read, FileShare.Read);
            if (file == null)
                return null;

            if (file.Length != 0)
            {
                SavedGame savedGame = ((SavedGame)new XmlSerializer(typeof(SavedGame)).Deserialize(file));

                file.Close();

                return savedGame;
            }

            return null;
        }

        #endregion

        #region ReadMap Method

        /// <summary>
        /// Read the content of a map with the specified name.
        /// </summary>
        /// <param name="mapName">Name of the map to be readed.</param>
        /// <returns>Dictionary with the information of the saved map.</returns>
        public static Dictionary<string, string> readMapInformation(string mapName)
        {
            // Create information structure
            Dictionary<string, string> information = new Dictionary<string, string>();

            // Open map file
            Stream stream = TitleContainer.OpenStream("Content/Maps/" + mapName);
            StreamReader reader = new StreamReader(stream);

            // Read the map name and insert into the dictionary
            string line = reader.ReadLine();
            information.Add("MapName", line);

            // Read the elements list and insert into the dictionary
            line = reader.ReadLine();
            information.Add("Elements", line);

            // Read the number of enemies and their data
            line = reader.ReadLine();
            information.Add("NumberOfEnemies", line);

            int enemies = Convert.ToInt32(line);
            for (int i = 0; i < enemies; i++)
            {
                line = reader.ReadLine();
                information.Add("Enemy" + i, line);
            }

            // Read objects information
            line = reader.ReadLine();
            information.Add("NumberOfObjects", line);

            int objects = Convert.ToInt32(line);
            for (int i = 0; i < objects; i++)
            {
                line = reader.ReadLine();
                information.Add("Object" + i, line);
            }

            // Read the number of layers and insert into the dictionary
            line = reader.ReadLine();
            information.Add("NumberOfLayers", line);

            // Read the layers and insert them into the dictionary
            int layerNumber = Convert.ToInt32(line);
            for (int i = 0; i < layerNumber; i++)
            {
                line = reader.ReadLine();
                information.Add("Layer" + i, line);
            }

            return information;
        }

        public static string[,] readMainLayer(string layerName)
        {
            Stream stream = TitleContainer.OpenStream("Content/Maps/" + layerName);
            StreamReader reader = new StreamReader(stream);

            // Read the layer dimensions
            string line = reader.ReadLine();
            string[] aux = line.Split(' ');
            int doors = Convert.ToInt32(aux[3]);
            string[,] layerInformation = new string[
                Convert.ToInt32(aux[0]) * Convert.ToInt32(aux[1]) + 3 + Convert.ToInt32(aux[3]), 
                Convert.ToInt32(aux[2]) + 1];

            layerInformation.SetValue(aux[0], 0, 0);
            layerInformation.SetValue(aux[1], 0, 1);
            layerInformation.SetValue(aux[2], 0, 2);

            // Read the origin
            line = reader.ReadLine();
            aux = line.Split(' ');

            layerInformation.SetValue(aux[0], 1, 0);
            layerInformation.SetValue(aux[1], 1, 1);
            layerInformation.SetValue(aux[2], 1, 2);

            // Read door information
            layerInformation.SetValue(doors.ToString(), 2, 0);

            for (int i = 0; i < doors; i++)
            {
                line = reader.ReadLine();
                aux = line.Split(' ');
                layerInformation.SetValue(aux[0], 3 + i, 0);
                layerInformation.SetValue(aux[1], 3 + i, 1);
                layerInformation.SetValue(aux[2], 3 + i, 2);
                layerInformation.SetValue(aux[3], 3 + i, 3);
                layerInformation.SetValue(aux[4], 3 + i, 4);
                layerInformation.SetValue(aux[5], 3 + i, 5);
                layerInformation.SetValue(aux[6], 3 + i, 6);
                layerInformation.SetValue(aux[7], 3 + i, 7);
                layerInformation.SetValue(aux[8], 3 + i, 8);
                layerInformation.SetValue(aux[9], 3 + i, 9);
                layerInformation.SetValue(aux[10], 3 + i, 10);
            }

            // Read the layer components
            int row = 0;
            int col = 0;
            line = reader.ReadLine();
            while (line != null)
            {
                if (line.Length == 0)
                {
                    line = reader.ReadLine();
                    continue;
                }
                string[] components = line.Split(' ');
                foreach (string component in components)
                {
                    layerInformation.SetValue(component, row + 3 + doors, col);
                    col++;
                }

                line = reader.ReadLine();
                col = 0;
                row++;
            }

            return layerInformation;
        }

        #endregion

        #region ReadGameLayer2D Method

        /// <summary>
        /// Read the content of a 2D layer.
        /// </summary>
        /// <param name="layerName">Name of the 2D layer to be readed.</param>
        /// <returns>Dictionary with the information of the 2D layer.</returns>
        public static string[,] ReadGameLayer2D(string layerName)
        {
            Stream stream = TitleContainer.OpenStream("Content/Maps/" + layerName);
            StreamReader reader = new StreamReader(stream);

            // Read the layer dimensions
            string line = reader.ReadLine();
            string[] aux = line.Split(' ');
            string[,] layerInformation = new string[Convert.ToInt32(aux[0]) + 5, Convert.ToInt32(aux[1])];

            layerInformation.SetValue(aux[0], 0, 0);
            layerInformation.SetValue(aux[1], 0, 1);

            // Read origin position
            line = reader.ReadLine();
            aux = line.Split(' ');

            layerInformation.SetValue(aux[0], 1, 0);
            layerInformation.SetValue(aux[1], 1, 1);
            layerInformation.SetValue(aux[2], 1, 2);

            // Read Up Vector
            line = reader.ReadLine();
            aux = line.Split(' ');

            layerInformation.SetValue(aux[0], 2, 0);
            layerInformation.SetValue(aux[1], 2, 1);
            layerInformation.SetValue(aux[2], 2, 2);

            // Read Normal Vector
            line = reader.ReadLine();
            aux = line.Split(' ');

            layerInformation.SetValue(aux[0], 3, 0);
            layerInformation.SetValue(aux[1], 3, 1);
            layerInformation.SetValue(aux[2], 3, 2);

            // Read texture name and number of textures
            line = reader.ReadLine();
            aux = line.Split(' ');

            layerInformation.SetValue(aux[0], 4, 0);
            layerInformation.SetValue(aux[1], 4, 1);

            // Read the layer components
            int row = 5;
            int col = 0;
            line = reader.ReadLine();
            while (line != null)
            {
                string[] components = line.Split(' ');
                foreach (string component in components)
                {
                    layerInformation.SetValue(component, row, col);
                    col++;
                }

                line = reader.ReadLine();
                col = 0;
                row++;
            }

            return layerInformation;
        }

        #endregion

        #region ReadGameLayer3D Method

        /// <summary>
        /// Read the content of a 3D layer.
        /// </summary>
        /// <param name="layerName">Name of the 3D layer to be readed.</param>
        /// <returns>Dictionary with the information of the 3D layer.</returns>
        public static string[,] ReadGameLayer3D(string layerName)
        {
            Stream stream = TitleContainer.OpenStream("Content/Maps/" + layerName);
            StreamReader reader = new StreamReader(stream);

            // Read the layer dimensions
            string line = reader.ReadLine();
            string[] aux = line.Split(' ');
            string[,] layerInformation = new string[Convert.ToInt32(aux[0]) * Convert.ToInt32(aux[1]) + 2, Convert.ToInt32(aux[2])];

            layerInformation.SetValue(aux[0], 0, 0);
            layerInformation.SetValue(aux[1], 0, 1);
            layerInformation.SetValue(aux[2], 0, 2);

            // Read the origin
            line = reader.ReadLine();
            aux = line.Split(' ');

            layerInformation.SetValue(aux[0], 1, 0);
            layerInformation.SetValue(aux[1], 1, 1);
            layerInformation.SetValue(aux[2], 1, 2);

            // Read the layer components
            int row   = 0;
            int col   = 0;
            line = reader.ReadLine();
            while (line != null)
            {
                if (line.Length == 0)
                {
                    line = reader.ReadLine();
                    continue;
                }

                string[] components = line.Split(' ');
                foreach (string component in components)
                {
                    layerInformation.SetValue(component, row+2, col);
                    col++;
                }

                line = reader.ReadLine();
                col = 0;
                row++;
            }

            return layerInformation;
        }

        #endregion
    }
}
