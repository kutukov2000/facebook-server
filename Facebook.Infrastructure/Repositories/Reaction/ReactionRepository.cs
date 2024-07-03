﻿using ErrorOr;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Domain.Reaction;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Reaction;

public class ReactionRepository(FacebookDbContext context) : IReactionRepository
{
    public async Task<ErrorOr<ReactionEntity>> AddReactionAsync(ReactionEntity reaction)
    {
        try
        {
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();
            return reaction;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteReactionAsync(Guid postId, Guid userId)
    {
        try
        {
            var reaction = await context.Reactions.SingleOrDefaultAsync(u => u.UserId == userId && u.PostId == postId);
            if (reaction == null)
            {
                return Error.Failure("Error");
            }

            context.Reactions.Remove(reaction);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
