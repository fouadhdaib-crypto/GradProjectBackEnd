using BusinessLayer.Dtos.Chat;
using DataAccessLayer;
using GradProject.Services;
using static DataAccessLayer.ClschatDataAcssesLayer;

namespace businessLayer_GP
{
    public class ClsChatBusinesssLayer
    {

        private readonly ClschatDataAcssesLayer _CLa;
        private readonly ICurrentUserService _ICurrentUserService;
        public ClsChatBusinesssLayer(ClschatDataAcssesLayer CLa, ICurrentUserService ICurrentUserService) {


            _CLa = CLa;
            _ICurrentUserService = ICurrentUserService;

        }

        public async Task<bool> CreateChatAsync(ChatRoomDto Crd) {



            var Userid = _ICurrentUserService.UserId;


            if (Userid < 1) {

                return false;
            
            }


            var Room = new ChatRoom { CreatedAt = Crd.CreatedAt, Name = Crd.Name, IsGroup = Crd.IsGroup, RoomKey = Crd.RoomKey };


           return     await  _CLa.CreateChatAsync(Room, Userid);




        }


        public async Task<bool> JoinChatRoom(int RoomId) {


            var Userid = _ICurrentUserService.UserId;
            if (Userid < 1)
            {

                return false;

            }

            return await _CLa.JoinChatRoom(RoomId, Userid);



        }


        public async Task<bool> SendMessageAsync(MessageDto Mdto)
        {

            var Userid = _ICurrentUserService.UserId;
            if (Userid < 1)
            {

                return false;

            }

            var Message = new Message { ChatRoomId = Mdto.ChatRoomId,
                SenderId = Mdto.SenderId,
                Content = Mdto.Content, 
                IsRead = Mdto.IsRead, 
                SentAt = DateTime.Now };


            return await _CLa.SendMessageAsync(Message);



        }

        public async Task<List<MessageDto>> GetAllMessageByRoomIdAsync(int RoomId) {


            List < MessageDto > MessagesDto = new List<MessageDto>();
                var Messages = await _CLa.GetAllMessageByRoomIdAsync(RoomId);


            foreach (var Item in Messages) {

                MessagesDto.Add(new MessageDto { Id =Item.Id,
                    IsRead =Item.IsRead ,
                    SenderId =Item.SenderId 
                    ,SentAt = Item.SentAt ,
                    ChatRoomId =Item.ChatRoomId ,
                    Content = Item.Content });



            }

            return MessagesDto;

        }


        public async Task<List<ChatUserDTO>> GetAllUserBySpecializationName(string SpecializationName)
        {




            return await _CLa.GetAllUserBySpecializationName(SpecializationName);



        }


        }
    }
