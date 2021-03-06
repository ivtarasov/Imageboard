﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Services.ImageHandling;
using Netaba.Services.Markup;
using Netaba.Services.Repository;
using Netaba.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        [Route("{boardId:int}", Name = "Board")]
        [Route("{boardId:int}/{treadId:int}", Name = "Tread")]
        public IActionResult CreatePost(int boardId, int? treadId)
        {
            if (treadId == null) return StartNewTread(boardId);
            else
            {
                if (_repository.IsThereBoard(boardId)) return ReplyToTread(treadId.Value);
                else return NotFound("Not found");
            }
        }   

        [HttpPost]
        [Route("CreatePost", Name = "CreatePost")]
        public IActionResult CreatePost(Post post, IFormFile file, int targetId, Destination dest)
        {
            if (post.IsOp) return StartNewTread(post, file, targetId, dest);
            else return ReplyToTread(post, file, targetId, dest);
        }

        [HttpPost]
        [Route("DeletePosts", Name = "DeletePosts")]
        public IActionResult DeletePosts(Dictionary<int, int> ids, int boardId, string password)
        {
            if (ids == null)
            {
                Console.WriteLine("null");
                return RedirectToRoute("Board", new { boardId });
            }

            _repository.DeletePosts(ids.Values, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            return RedirectToRoute("Board", new { boardId });
        }

        [NonAction]
        public IActionResult ReplyToTread(int treadId)
        {
            var tread = _repository.LoadTread(treadId);
            if (tread == null) return NotFound("Not found");
            else return View(new CreatePostViewModel( new List<TreadViewModel>{ new TreadViewModel(tread) }, 
                ReplyFormAction.ReplyToTread, tread.BoardId.Value, tread.Id));
        }

        [NonAction]
        public IActionResult ReplyToTread(Post post, IFormFile file, int treadId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var tread = _repository.LoadTread(treadId);
                return View(new CreatePostViewModel(new List<TreadViewModel>{ new TreadViewModel(tread) }, 
                    ReplyFormAction.ReplyToTread, post, tread.BoardId.Value, treadId));
            }

            //post.Image = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);

            //Image img = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            //var post = new Post(_parser.ToHtml(message), title, DateTime.Now, img, false, isSage, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            var  boardId = _repository.AddNewPostToTread(post, treadId);

            if (dest == Destination.Tread) return RedirectToRoute("Tread", new { boardId, treadId});
            else return RedirectToRoute("Board", new { boardId });
        }

        [NonAction]
        public IActionResult StartNewTread(int boardId)
        {
            var board = _repository.LoadBoard(boardId);
            if (board == null) return NotFound("Not found");
            else return View(new CreatePostViewModel(board.Treads.Select(t => new TreadViewModel(t, 11)).ToList(), 
                ReplyFormAction.StartNewTread, board.Id));
        }

        [NonAction]
        public IActionResult StartNewTread(Post post, IFormFile file, int boardId, Destination dest)
        {
            if (!ModelState.IsValid)
            {
                var board = _repository.LoadBoard(boardId);
                return View(new CreatePostViewModel(board.Treads.Select(t => new TreadViewModel(t, 11)).ToList(), 
                    ReplyFormAction.StartNewTread, post, boardId));
            }

            //post.Image = _imageHandler.HandleImage(file, _appEnvironment.WebRootPath);
            //var oPost = new Post(_parser.ToHtml(message), title, DateTime.Now, img, true, isSage, HttpContext.Connection.RemoteIpAddress.ToString(), password);
            var tread = new Tread(new List<Post> { post });
            var treadId = _repository.AddNewTreadToBoard(tread, boardId);

            if (dest == Destination.Board) return RedirectToRoute("Board", new { boardId });
            else return RedirectToRoute("Tread", new { boardId, treadId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
