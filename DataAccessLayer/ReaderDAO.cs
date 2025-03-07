using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class ReaderDAO
    {
        private readonly LibraryManagementContext _context;

        public ReaderDAO(LibraryManagementContext context)
        {
            _context = context;
        }

        public void AddReader(Reader reader)
        {
            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public Reader? GetReaderById(int id)
        {
            return _context.Readers.Find(id);
        }

        public Reader? GetReaderByPhoneNumber(string phoneNumber)
        {
            return _context.Readers.FirstOrDefault(r => r.PhoneNumber == phoneNumber);
        }

        public Reader? GetReaderByEmail(string email)
        {
            return _context.Readers.FirstOrDefault(r => r.Email == email);
        }

        public void Update(Reader reader)
        {
            _context.Readers.Update(reader);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var reader = _context.Readers.Find(id);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.SaveChanges();
            }
        }

        public List<Reader> GetAll()
        {
            return _context.Readers.ToList();
        }

        public List<User> GetReadersByFullName(string keyword)
        {
            return _context.Users
                .Where(r => r.Reader.FullName.ToLower().Contains(keyword.ToLower()))
                .ToList();
        }
    }
}