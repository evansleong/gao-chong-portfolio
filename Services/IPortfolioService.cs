using GaoChongPortfolio.Models;

namespace GaoChongPortfolio.Services
{
    public interface IPortfolioService
    {
        PortfolioData GetData();
        void SaveData(PortfolioData data);
    }
}
