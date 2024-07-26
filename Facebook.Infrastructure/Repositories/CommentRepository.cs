﻿using ErrorOr;
using Facebook.Application.Common.Interfaces.Comment.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Comment;

public class CommentRepository(FacebookDbContext context) : Repository<CommentEntity>(context), ICommentRepository
{

    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByPostIdAsync(Guid postId)
    {
        var comment = await context.Comments.Where(comment => comment.PostId == postId).ToListAsync();
        if (!comment.Any())
        {
            return Error.NotFound();
        }
        return comment;
    }

    public async Task<ErrorOr<IEnumerable<CommentEntity>>> GetCommentsByUserIdAsync(Guid userId)
    {
        var comment = await context.Comments.Where(comment => comment.UserId == userId).ToListAsync();
        if (!comment.Any())
        {
            return Error.NotFound();
        }
        return comment;
    }

    public async Task<ErrorOr<MediatR.Unit>> UpdateCommentAsync(CommentEntity comment)
    {
        try
        {
            var commentExist = await context.Comments.FindAsync(comment.Id);

            if (commentExist == null)
            {
                return Error.Failure("Comment not found");
            }

            commentExist.Message = comment.Message;

            context.Comments.Update(commentExist);
            await context.SaveChangesAsync();

            return MediatR.Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
