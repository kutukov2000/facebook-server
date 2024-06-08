using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Domain.Post;

namespace Facebook.Application.Post.Query.GetById;

public class GetPostByIdQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostByIdQuery, ErrorOr<PostEntity>>
{
    public async Task<ErrorOr<PostEntity>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await postRepository.GetPostByIdAsync(request.Id);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving post by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving post by id {request.Id}: {ex.Message}");
        }
    }
}