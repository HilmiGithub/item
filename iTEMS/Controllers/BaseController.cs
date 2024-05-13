using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTEMS.Data;
using iTEMS.Models;

namespace iTEMS.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        protected async Task SetNotificationsInViewBag()
        {
            var notifications = await _context.Notifications.OrderByDescending(n => n.Timestamp).ToListAsync();
            ViewBag.Notifications = notifications;
        }
    }
}
