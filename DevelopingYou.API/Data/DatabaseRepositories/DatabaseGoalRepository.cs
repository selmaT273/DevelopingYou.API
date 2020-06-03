﻿using DevelopingYou.API.Data.Interfaces;
using DevelopingYou.API.Models;
using DevelopingYou.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopingYou.API.Data.DatabaseRepositories
{
    public class DatabaseGoalRepository : IGoalRepository
    {
        private readonly DiscoveringYouDBContext _context;

        public DatabaseGoalRepository(DiscoveringYouDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GoalDTO>> GetGoals()
        {
            var goals = await _context.Goal
                .Select(goal => new GoalDTO
                {
                    Id = goal.Id,
                    UserId = goal.UserId,
                    Title = goal.Title,
                    StartValue = goal.StartValue,
                    TargetValue = goal.TargetValue,
                    StartDate = goal.StartDate,
                    EndDate = goal.EndDate,
                    Category = goal.Category,
                    Instances = goal.Instances
                    .Select(instance => new InstanceDTO
                    {
                        Id = instance.Id,
                        StartTime = instance.StartTime,
                        EndTime = instance.EndTime,
                        Comment = instance.Comment,

                    })
                .ToList()
                })
                .ToListAsync();

            return goals;
        }

        public async Task<GoalDTO> GetGoalById(int id)
        {
            var goal = await _context.Goal
                .Select(goal => new GoalDTO
                {
                    Id = goal.Id,
                    UserId = goal.UserId,
                    Title = goal.Title,
                    StartValue = goal.StartValue,
                    TargetValue = goal.TargetValue,
                    StartDate = goal.StartDate,
                    EndDate = goal.EndDate,
                    Category = goal.Category,
                    Instances = goal.Instances
                    .Select(instance => new InstanceDTO
                    {
                        Id = instance.Id,
                        GoalTitle = instance.GoalTitle,
                        StartTime = instance.StartTime,
                        EndTime = instance.EndTime,
                        Comment = instance.Comment,

                    })
                .ToList()
                })
                .FirstOrDefaultAsync(goal => goal.Id == id);

            return goal;
        }

        public async Task<Goal> SaveNewGoal(Goal goal)
        {
            _context.Goal.Add(goal);
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task<bool> UpdateGoal(int id, Goal goal)
        {
            _context.Entry(goal).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        private bool GoalExists(int id)
        {
            return _context.Goal.Any(g => g.Id == id);
        }

        public async Task<Goal> DeleteGoal(int id)
        {
            var goal = await _context.Goal.FindAsync(id);
            if (goal == null)
            {
                return null;
            }
            _context.Goal.Remove(goal);
            await _context.SaveChangesAsync();
            return goal;
        }
    }
}
