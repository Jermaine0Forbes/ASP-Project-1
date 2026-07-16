using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp2.Data;
using WebApp2.Models;
using WebApp2.Brokers;
using WebApp2.Managers;

namespace WebApp2.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDBContext _context;
        private readonly WebSocketsManager _manager;

        private readonly DataNotificationBroker _broker;

        public PostController(AppDBContext context, WebSocketsManager manager, DataNotificationBroker broker)
        {
            _context = context;
            _broker = broker;
            _manager = manager;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Posts.Include(p => p.User);
            return View(await appDBContext.ToListAsync());
        }

        [HttpGet("/ws")]
        public async Task ViewSocket()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var bytes = Encoding.UTF8.GetBytes("hello world");


                var connectionId = _manager.AddSocket(webSocket);

                // Define what happens IMMEDIATELY when the broker publishes an update
                Func<object, Task> pushHandler = async (payload) =>
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        var json = JsonSerializer.Serialize(payload);
                        var bytes = Encoding.UTF8.GetBytes(json);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                };

                // Subscribe this specific socket connection to the broker
                _broker.OnDataChanged += pushHandler;


                try
                {
                    var buffer = new byte[1024];
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket
                            .CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

                        }

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            var message = JsonSerializer.Deserialize<PostSocketModel>(json);
                            var id = message?.Id;
                            // string data = "";
                            if (message?.Status == "viewed")
                            {
                                var post = await _context.Posts.FindAsync(id) ?? throw new Exception("post not found");
                                post.Views += 1;
                                _context.Update(post);
                                await _context.SaveChangesAsync();
                               var data = new
                                {
                                    Status = "viewed",
                                    Id = id,
                                    Value = post.Views
                                };
                                await _broker.PublishUpdateAsync(data);


                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    // Clean up: unsubscribe from event and remove from manager to prevent memory leaks
                    _broker.OnDataChanged -= pushHandler;
                    await _manager.RemoveSocket(connectionId);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,CreatedAt,UpdatedAt,Views,Likes,UserId")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", post.UserId);
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", post.UserId);
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,CreatedAt,UpdatedAt,Views,Likes,UserId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", post.UserId);
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
