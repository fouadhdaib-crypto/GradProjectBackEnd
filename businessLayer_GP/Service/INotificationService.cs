using businessLayer_GP.Dtos.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace businessLayer_GP.Service
{
    public interface INotificationService
    {

        Task<bool> AddAsync(CreateNotificationDto dto);
        Task<bool> UpdateAsync(UpdateNotificationDto dto);
        Task<List<GetNotificationDto>>  GetAsync();
    }
}
