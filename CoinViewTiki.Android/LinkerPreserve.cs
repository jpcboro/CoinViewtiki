using Akavache.Sqlite3;

namespace CoinViewTiki.Droid
{
   
        public static class LinkerPreserve
        {
            static LinkerPreserve()
            {
                var persistentName = typeof(SQLitePersistentBlobCache).FullName;
                var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
            }
        }
    
}