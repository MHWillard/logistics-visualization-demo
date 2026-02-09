using Microsoft.EntityFrameworkCore;
using logistics_visualization_demo.Models;
using System;
using System.Collections.Generic;

namespace logistics_visualization_demo.Data
{
    public class RecordContext : DbContext
    {
        public DbSet<Record> Records { get; set; }

        public string DbPath { get; }

        public RecordContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "recordstore.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
