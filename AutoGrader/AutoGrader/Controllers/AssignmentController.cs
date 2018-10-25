﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGrader.DataAccess;
using AutoGrader.Models.Assignment;
using AutoGrader.Models.Submission;
using AutoGrader.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ShellHelper;

namespace AutoGrader.Controllers
{
    public class AssignmentController : Controller
    {
        private AutoGraderDbContext dbContext;

        public AssignmentController(AutoGraderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult CreateAssignment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(AssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Assignment assignment = new Assignment(model);
                AssignmentDataService assignmentDataService = new AssignmentDataService(dbContext);
                assignmentDataService.AddAssignment(assignment);
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SubmitAssignment(int Id)
        {
            SubmissionInputViewModel model = new SubmissionInputViewModel
            {
                AssignmentId = Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(SubmissionInputViewModel input)
        {
            if (ModelState.IsValid)
            {
                Submission submission = new Submission();

                submission.Input.SourceCode = input.SourceCode;
                submission.Input.Language = "C++";
                submission.AssignmentId = input.AssignmentId;

                SubmissionService submissionService = new SubmissionService(dbContext);
                submissionService.AddSubmission(submission);
                await dbContext.SaveChangesAsync(); // SubmissionId now set on local submission


                AssignmentDataService assignmentDataService = new AssignmentDataService(dbContext);
                Assignment assignment = assignmentDataService.GetAssignmentById(submission.AssignmentId);

                assignment.Submissions.Add(submission.Input);

                if (submission.Compile())
                {
                    for (int i = 0; i < submission.Output.TestCases.Count; i++)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        submission.Run(i);
                        watch.Stop();
                        if (submission.Output.Runtime < (double)watch.ElapsedMilliseconds)
                        {
                            submission.Output.Runtime = (double)watch.ElapsedMilliseconds;
                        }
                    }
                }
                else
                {
                    submission.Output.Compiled = false;
                }

                dbContext.Assignments.Update(assignment);
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction("StudentHome", "Student");
        }
    }
}
