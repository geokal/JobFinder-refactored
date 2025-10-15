using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using QuizManager.Models;
using System;
using System.IO;
using static System.Net.WebRequestMethods;

namespace QuizManager.Data
{
	public class FileUploadService 
	{
		private readonly IDbContextFactory<AppDbContext> _contextFactory;

		public FileUploadService(IDbContextFactory<AppDbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task UploadFiles(IList<Student> fileInfos)
		{
            using (var _context = _contextFactory.CreateDbContext())
			{ 
				foreach (var file in fileInfos) 
				{
					if (file.Id == 0)
					{
						_context.Students.Add(file);	
					}
				}
				await _context.SaveChangesAsync();	
			}
		}

		public async Task<Student> GetStudentByName(string name)
		{
			using (var _context = _contextFactory.CreateDbContext())
			{
				return await _context.Students.FirstOrDefaultAsync(s => s.Name == name);
			}
		}

        public async Task<Student> GetStudentByRegNumber(long regnumber)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Students.FirstOrDefaultAsync(s => s.RegNumber == regnumber);
            }
        }


    }
}
