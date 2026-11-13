using Ecom_Demo.Models;
using System.Threading.Tasks;

namespace Ecom_Demo.Services
{
    /// Question 2: Service Interface
    /// Defines contract for notification service (email, SMS)
    public interface INotificationService
    {
        Task SendOrderConfirmationEmailAsync(int orderId, string customerEmail);
        Task SendOrderStatusUpdateAsync(int orderId, OrderStatus status,
                                       string customerEmail);
    }
}