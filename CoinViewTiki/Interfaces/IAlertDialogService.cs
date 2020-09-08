using System.Threading.Tasks;

namespace CoinViewTiki.Interfaces
{
    public interface IAlertDialogService
    {
        void ShowAlertMessage(string title, string message);
    }
}