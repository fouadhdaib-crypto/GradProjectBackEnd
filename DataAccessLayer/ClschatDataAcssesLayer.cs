using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClschatDataAcssesLayer
    {

        private readonly AppDbContext _context;

        public class ChatUserDTO
        {
            public int? Id { get; set; }
            public string? UserName { get; set; }
            public string? imagePath { get; set; }
        }
        public ClschatDataAcssesLayer(AppDbContext context) {


            _context = context;



        }


        //Chat Room Part ///////////////////////


        public async Task<bool> CreateChatAsync(ChatRoom Room , int UserID) {


            var RoomId = await _CreateChatRoomAsync(Room);

            bool Result = false;
            if (RoomId > 0) {

                Result = await JoinChatRoom(Room.Id , UserID);

            }


            return Result;

        }



        private async Task<int> _CreateChatRoomAsync(ChatRoom Room) {


            var Result = await _context.ChatRooms.AddAsync(Room);

             await _context.SaveChangesAsync();

            return Room.Id;

        }


        public async Task<bool> JoinChatRoom(int RoomId, int UserID) {


            if (RoomId < 1 && UserID < 1) {

                return false;
            
            }
            var Result = 0;
            var ChatRoomUser = new ChatRoomUser { ChatRoomId = RoomId, JoinedAt = DateTime.UtcNow, UserId = UserID };

            await _context.ChatRoomUsers.AddAsync(ChatRoomUser);

            Result = await _context.SaveChangesAsync();


            return Result > 0;

        }

        public async Task<bool> SendMessageAsync(Message Object) {


            if (Object.ChatRoomId < 1) {

                return false;
            
            }

            await _context.Messages.AddAsync(Object);

            var Result = await _context.SaveChangesAsync();

            return Result > 0;
        }

        public async Task<List<Message>> GetAllMessageByRoomIdAsync(int RoomId)
        {


            if (RoomId < 1) {


                return new List<Message>();
            
            }


            var Message = await _context.Messages.
                Where(x => x.ChatRoomId == RoomId).
                ToListAsync();


            return Message;


        }





        public async Task<List<ChatUserDTO>> GetAllUserBySpecializationName(string SpecializationName)
        {





            var result = await _context.Users
                .Join(_context.Specializations,
                    user => user.SpecializationId,
                    spec => spec.Id,
                    (user, spec) => new { user, spec })
                .Where(x => x.spec.Name == SpecializationName)
                .Select(x => new ChatUserDTO
                {
                    Id = x.user.Id,
                    UserName = x.user.UserName,
                    imagePath = x.user.ImagePath,
                    
                })
                .ToListAsync();

            return result;
        }
    }
}
