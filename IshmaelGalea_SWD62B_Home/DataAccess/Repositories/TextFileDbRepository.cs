using DataAccess.context;
using Domain.Models;
using System;
using System.Linq;

namespace DataAccess.Repositories
{
    public class TextFileDbRepository
    {
        private FileSharingContext fileSharingContext;
        public TextFileDbRepository(FileSharingContext _fileSharingContext)
        {
            fileSharingContext = _fileSharingContext;
        }

        public void CreateFile(TextFile textfile)
        {
            fileSharingContext.TextFiles.Add(textfile);
            fileSharingContext.SaveChanges();
        }

        public void EditFile(Guid filename, string changes, TextFile updated)
        {
            var original = GetFile(updated.FileName);
            original.FileName = filename;
            original.UploadedOn = DateTime.Now;
            original.Data = changes;
            original.LastUpdated = updated.LastUpdated;
            original.LastEditedBy = updated.LastEditedBy;
            fileSharingContext.SaveChanges();
        }

        public void ShareFile(Guid fileId, string recipient, Acl acl)
        {
            var recipientAcl = fileSharingContext.Acls.Where(x => x.FileName == fileId && x.UserName == recipient).FirstOrDefault();
            acl.FileName = fileId;
            acl.UserName = recipient;
            fileSharingContext.Add(acl);
            fileSharingContext.SaveChanges();
        }

        public TextFile GetFile(Guid id)
        {
            return GetFiles().SingleOrDefault(x => x.FileName == id);
        }

        public IQueryable<TextFile> GetFiles()
        {
            return fileSharingContext.TextFiles;
        }

        public IQueryable<Acl> GetUsers()
        {
            return fileSharingContext.Acls;
        }

        public IQueryable<Acl> GetPermissions()
        {
            return fileSharingContext.Acls;
        }

        public void CreatePermissions(Acl acl)
        {
            fileSharingContext.Acls.Add(acl);
            fileSharingContext.SaveChanges();
        }       
    }
}