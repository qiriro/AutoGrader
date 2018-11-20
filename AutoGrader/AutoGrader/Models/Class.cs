﻿using AutoGrader.Models.Users;
using System.Collections.Generic;
using AutoGrader.Models.Assignment;
using AutoGrader.Models.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGrader.Models
{
    public class Class
    {
        public Class()
        {
            Assignments = new List<Assignment.Assignment>();
        }

        public Class(ClassViewModel model)
        {
            Assignments = new List<Assignment.Assignment>();

            this.Name = model.Name;
            this.ClassKey = model.ClassKey;
            this.InstructorId = model.InstructorId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassKey { get; set; }
        public int InstructorId { get; set; }
        public List<Assignment.Assignment> Assignments { get; set; }

        [NotMapped]
        public List<Student> Students { get; set; }
    }
}