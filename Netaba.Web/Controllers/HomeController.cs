﻿using Netaba.Data.Enteties;
using Netaba.Data.Enums;
using Netaba.Web.Models.ViewModels;
using Netaba.Services.Markup;
using Netaba.Services.ImageHandling;
using Netaba.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IParser _parser;
        private readonly IImageHandler _imageHandler;
        private readonly IWebHostEnvironment _appEnvironment;
        
        public HomeController(IRepository repository, IParser parser, IImageHandler imageHandler, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _parser = parser;
            _imageHandler = imageHandler;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult CreatePost(int? boardId = null, int? treadId = null)
        {
            if (treadId == null) return StartNewTread(boardId.Value);
            else return ReplyToTread(treadId.Value);
        }

        [HttpPost]
        public IActionResult CreatePost(Post post, string password, IFormFile file, int targetId, Destination dest)
        {
            if (post.IsOp) return StartNewTread(post, password, file, targetId, dest);
            else return ReplyToTread(post, password, file, targetId, dest);
        }

        [HttpPost]
        public IActionResult DeletePosts(Dictionary<int, int> ids, int boardId, string password)
        {
            _repository.DeletePosts(ids.Values, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            return StartNewTread(boardId);
        }

        [NonAction]
        public IActionResult ReplyToTread(int id)
        {
            var tread = _repository.LoadTread(id);
            if (tread == null) return NotFound("Not found");
            else return View(new CreatePostViewModel( new List<TreadViewModel>{ new TreadViewModel(tread) }, ReplyFormAction.ReplyToTread, id));
        }

        [NonAction]
        public IActionResult ReplyToTread(Post post, string password, IFormFile file, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("HeheTread");
                return View(new CreatePostViewModel(new List<TreadViewModel>{ new TreadViewModel(_repository.LoadTread(treadId)) }, ReplyFormAction.ReplyToTread, post, treadId));
            }

            post.Time = DateTime.Now;
            post.Image = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);

            //Image img = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            //var post = new Post(_parser.ToHtml(message), title, DateTime.Now, img, false, isSage, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            _repository.AddNewPost(post, treadId);

            if (dest == Destination.Tread) return View(new CreatePostViewModel(new List<TreadViewModel> { new TreadViewModel(_repository.LoadTread(treadId)) }, ReplyFormAction.ReplyToTread, treadId));
            else return StartNewTread(post.Tread.BoardId);
        }

        [NonAction]
        public IActionResult StartNewTread(int id)
        {
            var board = _repository.LoadBoard(id);
            if (board == null) return NotFound("Not found");
            else return View(new CreatePostViewModel(board.Treads.Select(t => new TreadViewModel(t, 11)).ToList(), ReplyFormAction.StartNewTread, id));
        }

        [NonAction]
        public IActionResult StartNewTread(Post post, string password, IFormFile file, int boardId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("HeheBoard");
                return View(new CreatePostViewModel(_repository.LoadBoard(boardId).Treads.Select(t => new TreadViewModel(t, 11)).ToList(), ReplyFormAction.StartNewTread, post, boardId));
            }

            post.Time = DateTime.Now;
            post.Image = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            //var oPost = new Post(_parser.ToHtml(message), title, DateTime.Now, img, true, isSage, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            var tread = new Tread(post);
            _repository.AddNewTreadToBoard(tread, boardId);

            if (dest == Destination.Board) return View(new CreatePostViewModel(_repository.LoadBoard(boardId).Treads.Select(t => new TreadViewModel(t, 11)).ToList(), ReplyFormAction.StartNewTread, boardId));
            else return RedirectToAction("ReplyToTread", new { id = tread.Id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
