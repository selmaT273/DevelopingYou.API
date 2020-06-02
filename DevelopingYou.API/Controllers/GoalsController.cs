﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevelopingYou.API.Data.Interfaces;
using DevelopingYou.API.Models;
using DevelopingYou.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DevelopingYou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        IGoalRepository goalRepository;

        public GoalsController(IGoalRepository goalRepository)
        {
            this.goalRepository = goalRepository;
        }

        //Get api/Goals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalDTO>>> GetGoals()
        {
            return Ok(await goalRepository.GetGoals());
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<GoalDTO>> GetGoalById(int id)
        {
            GoalDTO goal = await goalRepository.GetGoalById(id);

            if(goal == null)
            {
                return NotFound();
            }

            return goal;
        }

        [HttpPost]
        public async Task<ActionResult<Goal>> PostGoal(Goal goal)
        {
            await goalRepository.SaveNewGoal(goal);

            return CreatedAtAction("GetGoal", new { id = goal.Id }, goal);
        }
    }
}