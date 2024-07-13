using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Facebook.Domain.Chat;

namespace Facebook.Infrastructure.Common.Persistence;

public class FacebookDbContext
    : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public FacebookDbContext() : base() { }
    public FacebookDbContext(DbContextOptions<FacebookDbContext> options) : base(options) { }

    public DbSet<StoryEntity> Stories { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<ImagesEntity> Images { get; set; }
    public DbSet<UserProfileEntity> UsersProfiles { get; set; }
    public DbSet<FriendRequestEntity> FriendRequests { get; set; }
    public DbSet<FeelingEntity> Feelings { get; set; }
    public DbSet<ReactionEntity> Reactions { get; set; }
    public DbSet<LikeEntity> Likes { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ChatEntity> Chats { get; set; }
    public DbSet<UserChatEntity> UserChats{ get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<StoryEntity>()
            .HasOne<UserEntity>()
            .WithMany(u => u.Stories)
            .HasForeignKey(s => s.UserId);

        builder.Entity<ImagesEntity>()
            .HasOne(i => i.Post)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PostId);

        builder.Entity<ReactionEntity>()
            .HasOne(r => r.PostEntity)
            .WithMany(p => p.Reactions)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ReactionEntity>()
            .HasOne(r => r.UserEntity)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LikeEntity>()
            .HasOne(l => l.PostEntity)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LikeEntity>()
            .HasOne(l => l.UserEntity)
            .WithMany()  
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CommentEntity>()
            .HasOne(c => c.PostEntity)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CommentEntity>()
            .HasOne(c => c.UserEntity)
            .WithMany()  
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PostEntity>()
            .HasOne<UserEntity>()
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId);

        builder.Entity<UserProfileEntity>()
            .HasOne(up => up.UserEntity)
            .WithOne()
            .HasForeignKey<UserProfileEntity>(up => up.UserId);

        builder.Entity<FriendRequestEntity>()
            .HasOne(fr => fr.Sender)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FriendRequestEntity>()
            .HasOne(fr => fr.Receiver)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<PostEntity>()
            .HasOne(p => p.Feeling)
            .WithMany()
            .HasForeignKey(p => p.FeelingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserChatEntity>()
            .HasKey(uc => new { uc.UserId, uc.ChatId });

        builder.Entity<UserChatEntity>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserChats)
            .HasForeignKey(uc => uc.UserId);

        builder.Entity<UserChatEntity>()
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uc => uc.ChatId);

        builder.Entity<MessageEntity>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId);

        builder.Entity<MessageEntity>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId);
    }
}