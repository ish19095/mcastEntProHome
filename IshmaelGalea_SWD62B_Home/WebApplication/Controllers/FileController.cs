using BusinessLogic.Services;
using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WebApplication.Controllers
{
    public class FileController : Controller
    {
        private FileService fileService;
        private TextFileDbRepository textFileDbRepository;
        private IWebHostEnvironment webHostEnvironment;
        private LogService log;

        public FileController(FileService _fileService, TextFileDbRepository textFileDbRepository,
                             IWebHostEnvironment _webHostEnvironment, LogService _log)
        {
            fileService = _fileService;
            this.textFileDbRepository = textFileDbRepository;
            this.webHostEnvironment = _webHostEnvironment;
            this.log = _log;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(TextFile file, IFormFile filePath)
        {
            try
            {
                if (filePath != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(filePath.FileName);
                    string absolutePath = webHostEnvironment.ContentRootPath + @"\Data\" + uniqueFileName;
                    using (var destinationFile = System.IO.File.Create(absolutePath))
                    {
                        filePath.CopyTo(destinationFile);
                    }
                    file.FilePath = "/Data/" + uniqueFileName;
                    string[] lines = System.IO.File.ReadAllLines(absolutePath);
                    file.Data = string.Join("", lines);
                }
                var createFile = fileService.CreateFile(Guid.NewGuid(), file.Data, file.Author, file.FilePath);
                fileService.CreatePermissions(createFile, file.Author, true);
                log.Log("File successfully created!", HttpContext.Connection.RemoteIpAddress.ToString(), file.Author);
                string msg = "File successfully created!";
                ViewBag.Message = msg;
            }
            catch (Exception e)
            {
                log.Log(e, HttpContext.Connection.RemoteIpAddress.ToString(), file.Author);
                string error = "File was not created!";
                ViewBag.Error = error;
            }
            return View();
        }

        public IActionResult ListFiles()
        {
            var listFiles = fileService.GetFiles();
            return View(listFiles);
        }

        [HttpGet]
        public IActionResult editFile(Guid id)
        {
            var currentFile = fileService.GetFile(id);
            ListFileViewModels myModel = new ListFileViewModels()
            {
                FileName = currentFile.FileName,
                Data = currentFile.Data
            };
            return View(myModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult editFile(Guid fileName, string changes, ListFileViewModels file)
        {
            try
            {
                if (textFileDbRepository.GetPermissions().Where(x => x.FileName == fileName && x.UserName == User.Identity.Name && x.UserAccess == true).FirstOrDefault() != null)
                {
                    file.FileName = fileName;
                    file.LastEditedBy = User.Identity.Name;
                    changes = file.Data;
                    file.DigitalSignature = Convert.ToBase64String(fileService.DigitalSign(changes));
                    fileService.EditFile(fileName, changes, file);
                    
                    log.Log("File successfully updated!", HttpContext.Connection.RemoteIpAddress.ToString(), User.Identity.Name);
                    string msg = "File successfully updated!";
                    ViewBag.Message = msg;
                }
                else
                    throw new Exception("User cannot edit the file!");
            }

            catch (Exception ex)
            {
                string error = "File was not updated!";
                ViewBag.Error = error;
                log.Log(ex, HttpContext.Connection.RemoteIpAddress.ToString(), file.Author);
            }
            return RedirectToAction("ListFiles");
        }

        [HttpGet]
        public IActionResult ShareFile(Guid fileId)

        {
            var file = fileService.GetFile(fileId);
            var acl = new Acl { FileName = file.FileName, UserName = User.Identity.Name };
            return View(acl);
        }

        [HttpPost]
        public IActionResult ShareFile(Guid fileId, string user)
        {
            try
            {
                var file = fileService.GetFile(fileId);
                if(file != null)
                {
                    fileService.ShareFile(fileId, user);
                    string msg = "File has been shared successfully";
                    ViewBag.Message = msg;
                }
            }
            catch(Exception ex)
            {
                string msg = "File was not shared successfully";
                ViewBag.Message = msg;
            }
            return RedirectToAction("ListFiles");
        }

        public IActionResult ListAcl()
        {
            var listAcl = fileService.GetUsers();
            return View(listAcl);
        }
    }
}