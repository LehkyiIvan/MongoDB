using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SocialNetworkApp1
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseService;
        private string _currentUserId;

        public MainWindow()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadNewsFeed();
        }

        public void SetUserId(string userId)
        {
            _currentUserId = userId;
            LoadNewsFeed();
        }

        private void LoadNewsFeed()
        {
            var newsFeed = _databaseService.GetNewsFeed();
            var formattedFeed = newsFeed.Select(post => new
            {
                UserId = post["userId"].AsString,
                Content = post["content"].AsString,
                CreatedAt = post.Contains("createdAt") && post["createdAt"].IsValidDateTime ? post["createdAt"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") : post["createdAt"].AsString,
                Likes = post.Contains("reactions") ? post["reactions"]["like"].AsInt32 : 0,
                PostId = post["postId"].AsString
            }).ToList();
            NewsFeedListBox.ItemsSource = formattedFeed;
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is string postId && !string.IsNullOrEmpty(postId) && !string.IsNullOrEmpty(_currentUserId))
            {
                _databaseService.AddLike(postId, _currentUserId);
                MessageBox.Show("Liked!");
                LoadNewsFeed();
            }
        }

        private void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string friendId = FriendIdTextBox.Text;

            if (string.IsNullOrEmpty(friendId))
            {
                MessageBox.Show("Please enter a valid friend ID.");
                return;
            }

            _databaseService.AddFriend(_currentUserId, friendId);
            MessageBox.Show("Friend added successfully!");
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            string postId = PostIdTextBox.Text;
            string commentContent = CommentTextBox.Text;

            if (string.IsNullOrEmpty(commentContent))
            {
                MessageBox.Show("Please enter a comment.");
                return;
            }

            if (string.IsNullOrEmpty(postId))
            {
                MessageBox.Show("Please enter a valid Post ID.");
                return;
            }

            _databaseService.AddComment(postId, _currentUserId, commentContent);
            MessageBox.Show("Comment added successfully!");
        }

        private void RefreshFeedButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewsFeed();
        }
    }
}
