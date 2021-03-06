using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Util;
using Mobilis.Lib.Messages;

namespace Mobilis.Lib.ViewModel
{
    public class DiscussionsViewModel
    {
        public List<Discussion> discussions {get; private set;}
        private DiscussionDao discussionDao;
        private UserDao userDao;
        private PostDao postDao;
        private PostService postService;
        private Discussion selectedDiscussion;

        public DiscussionsViewModel() 
        {
            discussionDao = new DiscussionDao();
            userDao = new UserDao();
            postDao = new PostDao();
            postService = new PostService();
            discussions = discussionDao.getDiscussionsFromClass(ContextUtil.Instance.Class);
        }

        public void logout() 
        {
            User user = userDao.getUser();
            user.token = null;
            userDao.addUser(user);        
        }

        public bool existPostsAtDiscussion(int discussionPosition) 
        {
            selectedDiscussion = discussions[discussionPosition];
            ContextUtil.Instance.Discussion = selectedDiscussion._id;
            return postDao.existPostsAtDiscussion(selectedDiscussion._id);
        }

        public void requestPosts() 
        {
            postService.getPosts(Constants.NewPostURL(Constants.OLD_POST_DATE), userDao.getToken(), r =>
            {
                if (r.hasError())
                {
                    ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.CONNECTION_ERROR)));
                }
                else
                {
                    postDao.insertPost(r.Value);
                    selectedDiscussion.nextPosts = ContextUtil.Instance.postsAfter;
                    selectedDiscussion.previousPosts = ContextUtil.Instance.postsBefore;
                    discussionDao.updateDiscussion(selectedDiscussion);
                    ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.FUTURE_POSTS_LOADED)));
                }
            });
        }
    }
}