using Miru.Core;

namespace Miru.Storages
{
    public interface IStorage
    {
        /// <summary>
        /// Root dir for your storage
        /// </summary>
        MiruPath StorageDir { get; }
        
        /// <summary>
        /// Where yout app stores ephemeral data. Every instance of your app has different contents
        /// </summary>
        MiruPath App { get; }
        
        /// <summary>
        /// Where your app assets are located. Must have the same content for every instance of your app
        /// Should be commited to git repository
        /// </summary>
        MiruPath Assets { get; }

        void Put(MiruPath remote, MiruPath source);
    }
}