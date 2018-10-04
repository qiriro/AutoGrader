﻿using AutoGrader.Models.Submission;
using AutoGrader.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoGrader.Models.Assignment
{
    public class Assignment
    {
        public Assignment()
        {
            Languages = new List<string>();
            TestCases = new List<TestCase>();
        }

        public Assignment(AssignmentViewModel model)
        {
            this.Name = model.Name;
            this.StartDate = model.StartDate;
            this.EndDate = model.EndDate;
            this.Description = model.Description;
            this.MemoryLimit = model.MemoryLimit;
            this.TimeLimit = model.TimeLimit;
            this.Languages = model.Languages;
            this.TestCases.Add(new TestCase(model.TestCase1Input, model.TestCase1Output));
            this.TestCases.Add(new TestCase(model.TestCase2Input, model.TestCase2Output));
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public List<TestCase> TestCases { get; set; }

        public int MemoryLimit { get; set; }

        public int TimeLimit { get; set; }

        public List<string> Languages { get; set; }

        public List<SubmissionInput> Submissions { get; set; }
    }
}