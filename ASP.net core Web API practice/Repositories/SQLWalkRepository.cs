﻿using Microsoft.EntityFrameworkCore;
using Walk_and_Trails_of_SA_API.Data;
using Walk_and_Trails_of_SA_API.Models.Domain;

namespace Walk_and_Trails_of_SA_API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly DatabaseContext databaseContext;

        public SQLWalkRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await databaseContext.Walks.AddAsync(walk);
            await databaseContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existingWalk=  await databaseContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            //CHECK

            if (existingWalk == null)
            {
                return null;
            }

            databaseContext.Walks.Remove(existingWalk);
            await databaseContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetALLAsync()
        {
             return await databaseContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .ToListAsync();

        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
             return await databaseContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateByIdAsync(Guid id, Walk walk)
        {
            var existingWalk =await databaseContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            //check

            if (existingWalk != null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInkm= walk.LengthInkm;
            existingWalk.WalkImaeUrl = walk.WalkImaeUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

          await databaseContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
