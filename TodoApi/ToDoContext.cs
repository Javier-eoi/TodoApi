using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi
{
    public class TodoContext : DbContext
    {
        public DbSet<ToDoItem> Items { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options) { }
    }
}