using System;
using System.Collections.Generic;
using System.Linq;

namespace Task2_SocialNetworkSimulation
{
    public struct Comment
    {
        public string AuthorNickname { get; }
        public string Text { get; }
        public DateTime CreatedAt { get; }

        public Comment(string authorNickname, string text)
        {
            AuthorNickname = authorNickname;
            Text = text;
            CreatedAt = DateTime.Now;
        }
    }

    public class Post
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; }
        public string Text { get; }
        public int Likes { get; private set; }
        public List<Comment> Comments { get; } = new List<Comment>();

        public Post(string text)
        {
            Text = text;
            CreationDate = DateTime.Now;
        }

        public void AddLike() => Likes++;

        public void AddComment(Comment comment) => Comments.Add(comment);
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public List<Post> Posts { get; } = new List<Post>();

        private static int _timeSimulator = -500;

        public User(string name, string email, string nickname)
        {
            Name = name;
            Email = email;
            Nickname = nickname.StartsWith("@") ? nickname : "@" + nickname;
        }

        public Post CreatePost(string text)
        {
            var post = new Post(text);

            post.CreationDate = DateTime.Now.AddMinutes(_timeSimulator);
            _timeSimulator += 45;

            Posts.Add(post);
            Console.WriteLine($"{Nickname} створив(-ла) новий пост: '{text}'");
            return post;
        }

        public void DeletePost(Post post)
        {
            if (Posts.Remove(post))
            {
                Console.WriteLine($"{Nickname} успішно видалив(-ла) свій пост.");
            }
            else
            {
                Console.WriteLine($"[Відмовлено] {Nickname} не може видалити чужий пост.");
            }
        }

        public void LikePost(Post post)
        {
            post.AddLike();
            Console.WriteLine($"{Nickname} вподобав(-ла) пост: '{post.Text}'");
        }

        public void CommentPost(Post post, string text)
        {
            post.AddComment(new Comment(Nickname, text));
            Console.WriteLine($"{Nickname} залишив(-ла) коментар: '{text}'");
        }

        public void ShowLatest5Posts(User targetUser)
        {
            Console.WriteLine($"\n--- Стрічка {targetUser.Nickname} (переглядає {Nickname}) ---");

            var latestPosts = targetUser.Posts.OrderBy(p => p.CreationDate).Take(5).ToList();

            if (!latestPosts.Any())
            {
                Console.WriteLine("Користувач ще нічого не публікував(-ла).\n");
                return;
            }

            foreach (var post in latestPosts)
            {
                Console.WriteLine($"\n[Пост] {post.CreationDate:dd.MM.yyyy HH:mm:ss}");
                Console.WriteLine($"Текст: {post.Text}");
                Console.WriteLine($"Вподобань: {post.Likes} | Коментарів: {post.Comments.Count}");

                foreach (var comment in post.Comments)
                {
                    Console.WriteLine($"  -> {comment.AuthorNickname} [{comment.CreatedAt:HH:mm:ss}]: {comment.Text}");
                }
            }

            Console.WriteLine("\n--- Кінець стрічки ---\n");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            User user1 = new User("Маргарита Гришина", "rita@example.com", "rita_dev");
            User user2 = new User("Олексій Петров", "olexiy@example.com", "@alex_p");
            User user3 = new User("Іван Мельник", "ivan@example.com", "ivan_m");

            Post firstPost = user1.CreatePost("Усім привіт! Це мій найперший допис.");
            Post secondPost = user1.CreatePost("Починаю глибоко вивчати C# та ООП.");
            Post thirdPost = user1.CreatePost("Програмування - це цікаво.");
            Post draftPost = user1.CreatePost("Цей пост я випадково написала...");
            Post fourthPost = user1.CreatePost("Хто вже користувався Visual Studio?");
            Post lastPost = user1.CreatePost("Нарешті вихідні, можна трохи відпочити!");

            Console.WriteLine();

            user2.LikePost(firstPost);
            user3.LikePost(firstPost);
            user2.CommentPost(firstPost, "Вітаю на платформі!");
            user3.CommentPost(firstPost, "Перший допис - це завжди хвилююче!");

            user3.LikePost(secondPost);
            user2.CommentPost(secondPost, "Успіхів у вивченні!");

            user2.LikePost(thirdPost);

            user3.LikePost(fourthPost);
            user3.CommentPost(fourthPost, "Я користуюсь, дуже зручно!");

            user2.LikePost(lastPost);
            user3.LikePost(lastPost);
            user2.CommentPost(lastPost, "Гарних вихідних!");
            user3.CommentPost(lastPost, "І тобі гарно відпочити!");

            Console.WriteLine();

            user1.DeletePost(draftPost);
            user2.DeletePost(firstPost);

            user2.ShowLatest5Posts(user1);
        }
    }
}