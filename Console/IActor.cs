using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Hqv.MediaTools.Console
{
    /// <summary>
    /// Actions
    /// </summary>
    internal interface IActor
    {
        bool ShouldAct(CreateThumbnailSheetOptions options);
        Task Act(CreateThumbnailSheetOptions options, IConfiguration configuration);
    }
}