using System;

namespace AppMvc6.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserIconPath { get; set; }

        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    //public class CommentFormModel
    //{
    //    public int Id { get; set; }
    //    public int? ParentId { get; set; }
    //    public string Content { get; set; }
    //}
}