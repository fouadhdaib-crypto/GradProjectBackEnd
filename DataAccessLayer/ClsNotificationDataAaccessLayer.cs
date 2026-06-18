using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ClsNotificationDataAaccessLayer
    {
        private readonly AppDbContext _context;

        public ClsNotificationDataAaccessLayer(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get Notifications by User
        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // ✅ Get By Id
        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        // ✅ Create
        public async Task<bool> CreateNotificationAsync(Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;

            _context.Notifications.Add(notification);
         var  Result =  await _context.SaveChangesAsync();

            return Result >0 ;
        }

        // ✅ Update
        public async Task<bool> UpdateAsync(Notification notification)
        {
            var existing = await _context.Notifications.FindAsync(notification.Id);

            if (existing == null)
                return false;

            existing.Title = notification.Title;
            existing.Message = notification.Message;
            existing.Type = notification.Type;
            existing.ProjectId = notification.ProjectId;

            var Result =  await _context.SaveChangesAsync();
            return Result > 0;
        }

        // ✅ Mark as Read
        public async Task<bool> MarkAsReadAsync(int id)
        {
            var n = await _context.Notifications.FindAsync(id);

            if (n == null)
                return false;

            n.IsRead = true;
            await _context.SaveChangesAsync();

            return true;
        }

        // ✅ Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var n = await _context.Notifications.FindAsync(id);

            if (n == null)
                return false;

            _context.Notifications.Remove(n);
            await _context.SaveChangesAsync();

            return true;
        }

        // ✅ Count Unread
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        // ✅ Delete All for User
        public async Task<bool> DeleteAllForUserAsync(int userId)
        {
            var list = await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            if (!list.Any())
                return false;

            _context.Notifications.RemoveRange(list);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}