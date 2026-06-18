using businessLayer_GP.Dtos.Notification;
using businessLayer_GP.Service;
using DataAccessLayer;
using GradProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace businessLayer_GP
{
    public class NotificationService: INotificationService
    {

        private readonly ClsNotificationDataAaccessLayer ClsNotificationDataAaccessLayer_;
        private readonly ICurrentUserService _ICurrentUserService;


        public NotificationService(ClsNotificationDataAaccessLayer _ClsNotificationDataAaccessLayer , ICurrentUserService ICurrentUserService) {



            ClsNotificationDataAaccessLayer_ = _ClsNotificationDataAaccessLayer;

            _ICurrentUserService = ICurrentUserService;




        }


 




        public async Task <bool> AddAsync(CreateNotificationDto _CreateNotificationDto) {

            var notification = new Notification
            {
                Type = _CreateNotificationDto.Type,
                SenderId = _CreateNotificationDto.SenderId,
                Title = _CreateNotificationDto.Title,
                Message = _CreateNotificationDto.Message,
                UserId = _CreateNotificationDto.UserId.Value,
                ProjectId = _CreateNotificationDto.ProjectId
            };


          return  await ClsNotificationDataAaccessLayer_.CreateNotificationAsync(notification);
        
        
        
        }

        public async Task<bool> UpdateAsync(UpdateNotificationDto _UpdateNotificationDto)
        {

            var notification = new Notification
            {
                Id = _UpdateNotificationDto.Id,
                Type = _UpdateNotificationDto.Type,
                Title = _UpdateNotificationDto.Title,
                Message = _UpdateNotificationDto.Message,
                ProjectId = _UpdateNotificationDto.ProjectId
            };

            var existing = await ClsNotificationDataAaccessLayer_.GetByIdAsync(_UpdateNotificationDto.Id);
            if (existing == null)
                return false;


            return  await ClsNotificationDataAaccessLayer_.UpdateAsync(notification);





        }


        public async  Task<List<GetNotificationDto>> GetAsync() {


            var userId = _ICurrentUserService.UserId;

            var notificationsList = new List<GetNotificationDto>();

            if (userId==0) {

                return notificationsList;
                
            }

            var Notifcation =  await ClsNotificationDataAaccessLayer_.GetByUserIdAsync(userId);


            if (Notifcation.Count == 0) {
                return notificationsList;

            }

            foreach (var item in Notifcation) {


              var  notification= new GetNotificationDto
                {

                    Message = item.Message
                  ,
                    ProjectId = item.ProjectId
                  ,
                    Title = item.Title
                  ,
                    Id = userId
                 ,
                    IsRead = item.IsRead
                 ,
                    CreatedAt = item.CreatedAt


                };

                notificationsList.Add(notification);

            }


            return notificationsList;





        }



    }
}
