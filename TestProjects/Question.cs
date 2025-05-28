using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjects
{
    public class Question
    {
        public string Text { get; set; }
        public string[] Answers { get; set; } = new string[4];
        public int CorrectAnswer { get; set; }
    }
}
