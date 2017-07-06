using Arcflix.Droid.DB;
using Arcflix.Services.DB;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using System;
using System.IO;

[assembly: Dependency(typeof(SQLiteDroid))]
namespace Arcflix.Droid.DB
{
    public class SQLiteDroid : ISQLite
    {
        #region ISQLite implementation
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "arcflix.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLiteConnection(new SQLitePlatformAndroidN(), path);

            // Return the database connection
            return conn;
        }
        #endregion

        /// <summary>
        /// helper method to get the database out of /raw/ and into the user file system
        /// </summary>
        void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }
    }
}