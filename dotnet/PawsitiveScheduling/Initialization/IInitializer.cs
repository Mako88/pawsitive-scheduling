using System.Threading.Tasks;

namespace PawsitiveScheduling.Initialization
{
    /// <summary>
    /// Interface for classes that perform initialization logic
    /// </summary>
    public interface IInitializer
    {
        /// <summary>
        /// Perform initialization
        /// </summary>
        Task Initialize();
    }
}
