﻿// **********************************************************************************
// The MIT License (MIT)
// 
// Copyright (c) 2014 Rob Prouse
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// **********************************************************************************

#region Using Directives

using System.Windows;
using System.Windows.Input;
using Alteridem.GitHub.Extension.Interfaces;
using Alteridem.GitHub.Extension.View;
using Octokit;

#endregion

namespace Alteridem.GitHub.Extension.ViewModel
{
    public class AddCommentViewModel : BaseViewModel
    {
        private readonly IClosable _closable;
        private readonly Issue _issue;
        private string _comment;

        public AddCommentViewModel(IClosable closable)
        {
            _closable = closable;
            _issue = GitHubApi.Issue;

            CloseIssueCommand = new RelayCommand(p => OnCloseIssue(), p => CanCloseIssue());
            CommentCommand = new RelayCommand(p => OnCommentOnIssue(), p => CanCommentOnIssue());
        }

        public Issue Issue
        {
            get { return _issue; }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value == _comment) return;
                _comment = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseIssueCommand { get; private set; }
        public ICommand CommentCommand { get; private set; }

        private void OnCloseIssue()
        {
            GitHubApi.CloseIssue(GitHubApi.Issue, Comment);
            _closable.Close();
        }

        private bool CanCloseIssue()
        {
            return _issue != null;
        }

        private void OnCommentOnIssue()
        {
            GitHubApi.AddComment(GitHubApi.Issue, Comment);
            _closable.Close();
        }

        private bool CanCommentOnIssue()
        {
            return _issue != null && !string.IsNullOrWhiteSpace(Comment);
        }
    }
}